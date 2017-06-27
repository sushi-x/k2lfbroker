using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.Xml;

using Laserfiche.RepositoryAccess;
using Laserfiche.DocumentServices;

namespace K2.LaserficheServiceObject.Data
{

    class LaserficheProvider
    {
        private RepositoryRegistration _currentRegistration = null;
        private Session _currentSession = null;
        private string _serverName = string.Empty;
        private string _repositoryName = string.Empty;

        public LaserficheProvider(string serverName, string repositoryName)
        {
            _serverName = serverName;
            _repositoryName = repositoryName;

        }

        public void Connect()
        {
            try
            {

                _currentRegistration = new RepositoryRegistration(_serverName, _repositoryName);
                _currentSession = new Session();

                _currentSession.LogIn(_currentRegistration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void Logout()
        {
            if (_currentSession != null)
            {
                if (_currentSession.LogInTime.Year.ToString() != "1")
                {
                    _currentSession.LogOut();
                }
                _currentSession = null;
                _currentRegistration = null;

            }
        }

        public static bool IsDate(Object obj)
        {
            if (obj == null)
                return false;
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<DocumentInfo> DocumentSearchByTemplate(string folderName, string templateName, FieldValueCollection fv)
        {
            try
            {
                List<DocumentInfo> documentList = new List<DocumentInfo>();
                string searchFolder = String.Format("{{LF:Lookin=\"{0}\"}}", folderName);
                string searchParameters = string.Empty;
                string searchFields = string.Empty;

                foreach (KeyValuePair<string, object> kvPair in fv)
                {
                    if (kvPair.Value.ToString().Contains(','))
                    {
                        // multivalue
                        string values = kvPair.Value.ToString();
                        string[] items = values.Split(',');
                        if (items.Length > 0)
                        {
                            int x = 0;
                            foreach (object o in (Array)items)
                            {
                                if (x == 0)
                                {
                                    string tempSearch = string.Format(" {{[{0}]:[{1}]>=\"{2}\"}}", templateName, kvPair.Key.ToString(), o.ToString());
                                    searchFields += tempSearch;
                                }
                                else
                                {
                                    string tempSearch = string.Format(" | {{[{0}]:[{1}]>=\"{2}\"}}", templateName, kvPair.Key.ToString(), o.ToString());
                                    searchFields += tempSearch;
                                }
                                x++;
                            }
                        }
                    }
                    else
                    {
                        if (IsDate(kvPair.Value.ToString()))
                        {
                            //date = do a greater than or equal
                            string tempSearch = string.Format(" & {{[{0}]:[{1}]>=\"{2}\"}}", templateName, kvPair.Key.ToString(), kvPair.Value.ToString());
                            searchFields += tempSearch;
                        }
                        else
                        {
                            string tempSearch = string.Format(" & {{[{0}]:[{1}]=\"{2}\"}}", templateName, kvPair.Key.ToString(), kvPair.Value.ToString());
                            searchFields += tempSearch;
                        }
                    }
                }

                searchParameters = searchFolder + searchFields;

                Search lfSearch = new Search(_currentSession, searchParameters);
                SearchListingSettings settings = new SearchListingSettings();
                settings.AddColumn(SystemColumn.Id);

                lfSearch.Run();

                SearchResultListing searchResults = lfSearch.GetResultListing(settings);

                foreach (EntryListingRow item in searchResults)
                {
                    Int32 docId = (Int32)item[SystemColumn.Id];

                    EntryInfo entryInfo = Entry.GetEntryInfo(docId, _currentSession);
                    if (entryInfo.EntryType == EntryType.Shortcut)
                        entryInfo = Entry.GetEntryInfo(((ShortcutInfo)entryInfo).TargetId, _currentSession);

                    // Now entry should be the DocumentInfo
                    if (entryInfo.EntryType == EntryType.Document)
                    {
                        documentList.Add((DocumentInfo)entryInfo);

                    }

                }
                return documentList;
            }
            catch (Exception ex)
            {
                this.Logout();
                throw ex;
            }

        }

        public DocumentInfo DocumentGetByEntryID(Int32 entryId)
        {
            try
            {
                EntryInfo entryInfo = Entry.GetEntryInfo(entryId, _currentSession);
                if (entryInfo.EntryType == EntryType.Shortcut)
                    entryInfo = Entry.GetEntryInfo(((ShortcutInfo)entryInfo).TargetId, _currentSession);

                // Now entry should be the DocumentInfo
                if (entryInfo.EntryType == EntryType.Document)
                    return (DocumentInfo)entryInfo;
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.Logout();
                throw ex;
            }
        }

        public DocumentInfo DocumentUpdateByEntryID(Int32 entryId, FieldValueCollection fv)
        {
            try
            {
                EntryInfo entryInfo = Entry.GetEntryInfo(entryId, _currentSession);
                if (entryInfo.EntryType == EntryType.Shortcut)
                    entryInfo = Entry.GetEntryInfo(((ShortcutInfo)entryInfo).TargetId, _currentSession);

                // Now entry should be the DocumentInfo
                if (entryInfo.EntryType == EntryType.Document)
                {
                    DocumentInfo docInfo = (DocumentInfo)entryInfo;

                    // update only values that have been provided
                    // in fv
                    FieldValueCollection docFieldValues = docInfo.GetFieldValues();
                    foreach (KeyValuePair<string, object> kvPair in fv)
                    {
                        docFieldValues.Remove(kvPair.Key.ToString());
                        docFieldValues.Add(kvPair.Key.ToString(),kvPair.Value);
                    }

                    docInfo.SetFieldValues(docFieldValues);
                    docInfo.Save();
                    return docInfo;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.Logout();
                throw ex;
            }
        }


        public DocumentInfo DocumentAddDocument(string folder, string documentName, string documentContents, string templateName, FieldValueCollection fv)
        {
            try
            {

                foreach (KeyValuePair<string, object> kvPair in fv)
                {
                    Console.WriteLine(kvPair.Key.ToString(), kvPair.Value.ToString());
                }

                FolderInfo parentFolder = Folder.GetFolderInfo(folder, _currentSession);
                DocumentInfo document = new DocumentInfo(_currentSession);
                document.Create(parentFolder, documentName, EntryNameOption.None);
                document.SetTemplate(templateName);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(documentContents);
                Byte[] bytes = Convert.FromBase64String(xmlDoc.SelectSingleNode("//file/content").InnerText);
                using (System.IO.Stream edocStream = document.WriteEdoc("application/pdf", bytes.ToArray().LongLength))
                {
                    edocStream.Write(bytes.ToArray(), 0, bytes.ToArray().Length);
                }
                document.Extension = ".pdf";

                document.SetFieldValues(fv);
                document.Save();
                return document;

                #region WriteToFileForDebugging
                //try
                //{
                //    // Create a directory in the current working directory.
                //    Directory.CreateDirectory("tempDir");
                //    TempFileCollection tfc = new TempFileCollection("tempDir", false);

                //    // Returns the file name relative to the current working directory.
                //    string fileName = tfc.AddExtension("pdf");

                //    // Create and use the test files.
                //    //Byte[] bytes = ASCIIEncoding.Default.GetBytes(documentContents);
                //    File.WriteAllText(fileName, documentContents);

                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(fileName));
                //    using (System.IO.Stream edocStream = document.WriteEdoc("application/pdf", ms.ToArray().LongLength))
                //    {
                //        edocStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
                //    }
                //    document.Extension = ".pdf";
                //}
                //catch (Exception ex)
                //{
                //    this.Logout();
                //    throw ex;
                //}
                //finally
                //{
                //    //tfc.Delete();
                //}
                #endregion

            }
            catch (Exception ex)
            {
                this.Logout();
                throw ex;
            }
        }

        public static System.IO.MemoryStream GenerateStreamFromString(string value)
        {
            //return new System.IO.MemoryStream(Encoding.GetEncoding(1250).GetBytes(value ?? ""));
            return new System.IO.MemoryStream(Encoding.GetEncoding(1250).GetBytes(value ?? ""));
        }

        public List<TemplateInfo> TemplatesGetAll()
        {
            try
            {
                List<TemplateInfo> templateList = new List<TemplateInfo>();
                foreach (TemplateInfo templateInfo in Template.EnumAll(_currentSession))
                {
                    templateList.Add(templateInfo);
                }
                return templateList;
            }
            catch (Exception ex)
            {
                this.Logout();
                throw ex;
            }
        }

        public FieldInfo TemplateGetFieldInfo(string fieldName)
        {
            return Field.GetInfo(fieldName, _currentSession);
        }

    }

}
