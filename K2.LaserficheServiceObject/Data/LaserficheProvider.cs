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
            if (_currentSession!=null){
                if (_currentSession.LogInTime.Year.ToString() != "1")
                {
                    _currentSession.LogOut();
                }
                _currentSession = null;
                _currentRegistration = null;

            }
        }

        public DocumentInfo DocumentGetByEntryID(Int32 entryId)
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

        public DocumentInfo DocumentUpdateByEntryID(Int32 entryId, FieldValueCollection fv)
        {
            EntryInfo entryInfo = Entry.GetEntryInfo(entryId, _currentSession);
            if (entryInfo.EntryType == EntryType.Shortcut)
                entryInfo = Entry.GetEntryInfo(((ShortcutInfo)entryInfo).TargetId, _currentSession);

            // Now entry should be the DocumentInfo
            if (entryInfo.EntryType == EntryType.Document)
            {
                DocumentInfo docInfo = (DocumentInfo)entryInfo;
                docInfo.SetFieldValues(fv);
                docInfo.Save();
                return docInfo;
            }
            else
                return null;

        }


        public List<TemplateInfo> TemplatesGetAll()
        {
            List<TemplateInfo> templateList = new List<TemplateInfo>();
            foreach (TemplateInfo templateInfo in Template.EnumAll(_currentSession))
            {
                templateList.Add(templateInfo);
            }
            return templateList;

        }


    }

    


}
