using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    string tempSearch = string.Format(" & {{[{0}]:[{1}]=\"{2}\"}}", templateName, kvPair.Key.ToString(), kvPair.Value.ToString());
                    searchFields += tempSearch;
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


        public DocumentInfo DocumentAddDocument(string folder, string documentName, string templateName, FieldValueCollection fv)
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

                document.SetFieldValues(fv);
                document.Save();
                return document;
            }
            catch (Exception ex)
            {
                this.Logout();
                throw ex;
            }
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
