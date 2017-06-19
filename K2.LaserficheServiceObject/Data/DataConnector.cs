using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

using Laserfiche.RepositoryAccess;
using Laserfiche.DocumentServices;
using K2.LaserficheServiceObject.Data;

using K2.LaserficheServiceObject.Interfaces;

namespace K2.LaserficheServiceObject.DataConnectors
{
    /// <summary>
    /// An implementation of IDataConnector responsible for interacting with an underlying system or technology. 
    /// The purpose of this class is to expose and represent the underlying data and services as Service Objects 
    /// which are, in turn, consumed by K2 SmartObjects
    /// TODO: implement the Interface Methods with your own code which interacts with the underlying provider
    /// </summary>
    class DataConnector : IDataConnector
    {
        #region Class Level Fields

        #region Constants
        /// <summary>
        /// Constant for the Type Mappings configuration lookup in the service instance.
        /// </summary>
        private const string TYPEMAPPINGS = "Type Mappings";
        #endregion

        #region Private Fields
        /// <summary>
        /// Local serviceBroker variable.
        /// </summary>
        private ServiceAssemblyBase serviceBroker = null;

        /// <summary>
        /// Sample configuration values for the service instance
        /// defined by the SetupConfiguration() method and set by the GetConfiguration() method
        /// </summary>
        private string _requiredLaserficheServerValue = string.Empty;
        private string _requiredLaserficheWorkflowServerValue = string.Empty;
        private string _requiredLaserficheRepositoryValue = string.Empty;
        private string _optionalConfigurationValue = string.Empty;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Instantiates a new DataConnector.
        /// </summary>
        /// <param name="serviceBroker">The ServiceBroker.</param>
        public DataConnector(ServiceAssemblyBase serviceBroker)
        {
            // Set local serviceBroker variable.
            this.serviceBroker = serviceBroker;
        }
        #endregion

        #region Interface Methods

        #region void SetupConfiguration()
        /// <summary>
        /// Sets up the required configuration parameters for the service instance. 
        /// When a new service instance is registered for this ServiceBroker, the configuration parameters are surfaced to the registration tool. 
        /// The configuration values are provided by the person registering the service instance. 
        /// You can mark the configuration properties as "required" and also provide a default value
        /// </summary>
        public void SetupConfiguration()
        {
            //TODO: Add the service instance configuration values here

            //In this example, we are adding two configuration values, one required and one optional, and one with a default value 
            serviceBroker.Service.ServiceConfiguration.Add("Laserfiche Server", true, "TEXAN-APP26");
            serviceBroker.Service.ServiceConfiguration.Add("Laserfiche Workflow Server", true, "TEXAN-APP9");
            serviceBroker.Service.ServiceConfiguration.Add("Laserfiche Repository", true, "Laserfiche");
        }
        #endregion

        #region void GetConfiguration()
        /// <summary>
        /// Retrieves the configuration from the service instance and stores the retrieved configuration in local variables for later use.
        /// </summary>
        public void GetConfiguration()
        {
            //TODO: Add code to retrieve the service instance configuration values

            //in this example, we are returning the two configuration values that were added by the SetupConfiguration() method
            //and saving them to local private variables for re-use by other methods

            //the required configuration value will always be there
            _requiredLaserficheServerValue = serviceBroker.Service.ServiceConfiguration["Laserfiche Server"].ToString();
            _requiredLaserficheWorkflowServerValue = serviceBroker.Service.ServiceConfiguration["Laserfiche Workflow Server"].ToString();
            _requiredLaserficheRepositoryValue = serviceBroker.Service.ServiceConfiguration["Laserfiche Repository"].ToString();

            //optional configuration values may not always exist, so check them first 
            if (serviceBroker.Service.ServiceConfiguration["OptionalConfigurationValue"] != null)
            {
                _optionalConfigurationValue = serviceBroker.Service.ServiceConfiguration["OptionalConfigurationValue"].ToString();
            }
            else
            {
                _optionalConfigurationValue = "";
            }
        }
        #endregion

        #region void SetupService()
        /// <summary>
        /// Sets up the service instance's default name, display name, and description.
        /// </summary>
        public void SetupService()
        {
            //TODO: Add code to set up the service instance name, display name and description
            //NOTE: "Name" cannot contain spaces
            serviceBroker.Service.Name = "K2.LaserficheServiceBroker";
            serviceBroker.Service.MetaData.DisplayName = "Houston Texans K2/Laserfiche ServiceBroker";
            serviceBroker.Service.MetaData.Description = "A service broker that provides various functional service objects that assist in the implementation of a K2 project.";
        }
        #endregion

        #region void DescribeSchema()
        /// <summary>
        /// Describes the schema of the underlying data and services to the K2 platform.
        /// This method is called when a Service Instance is Registered and Refreshed, and 
        /// is responsible for defining the Service Objects that are available in Service Instance.
        /// </summary>
        public void DescribeSchema()
        {
            //TODO: Add code to define one or more ServiceObjects and Add each ServiceObject to the serviceBroker.Service.ServiceObjects collection
            //In a real-world implementation you would connect to the Provider and then run code to discover the Provider's Schema to determine the Objects, their Properties and their Methods.
            //for each Object you would define a ServiceObject with a collection of Property and a collection of Methods

            //OBTAINING THE SECURITY CREDENTIALS FOR A SERVICE INSTANCE AT RUNTIME
            //if you need to obtain the authentication credentials (username/password) for the service instance, query the following properties:
            //Note: password may be blank unless you are using Static or SSO credentials
            //string username = serviceBroker.Service.ServiceConfiguration.ServiceAuthentication.UserName;
            //string password = serviceBroker.Service.ServiceConfiguration.ServiceAuthentication.Password;

            //OBTAINING CONFIGURATION SETTINGS FOR A SERVICE AT RUNTIME
            //if you need to obtain configuration settings for the service instance and you did not define local members, query the following property:
            //string configValue = serviceBroker.Service.ServiceConfiguration["ConfigItemName"].ToString();

            ////in this sample, we are calling a sample helper method called GenerateServiceObjects() to define and return two sample service objects
            ////refer to the helper method to see how Service Objects are defined, but you will need to implement your own code in a real-world situation
            //foreach (ServiceObject svcObj in GenerateServiceObjects())
            //{
            //    serviceBroker.Service.ServiceObjects.Create(svcObj);
            //}


            ////ServiceObject dso = serviceBroker.Service.ServiceObjects.Create(GenerateDocumentServiceObject());
            serviceBroker.Service.ServiceObjects.Create(GenerateDocumentServiceObject());

            ////ServiceObject tso = serviceBroker.Service.ServiceObjects.Create(GenerateTemplatesServiceObject());
            ////ServiceObject tso2 = serviceBroker.Service.ServiceObjects.Create(GenerateTemplatesServiceObject2());
            ////ServiceObject tso3 = serviceBroker.Service.ServiceObjects.Create(GenerateTemplatesServiceObject3());

            ////ServiceObject wfo = serviceBroker.Service.ServiceObjects.Create(GenerateWorkflowsServiceObject());
            ////ServiceObject wfo2 = serviceBroker.Service.ServiceObjects.Create(GenerateWorkflowsServiceObject2());
            ////ServiceObject wfo3 = serviceBroker.Service.ServiceObjects.Create(GenerateWorkflowsServiceObject3());


            ////serviceBroker.Service.ServiceObjects.Create(dso);

            ////serviceBroker.Service.ServiceObjects.Create(tso);
            ////serviceBroker.Service.ServiceObjects.Create(tso2);
            ////serviceBroker.Service.ServiceObjects.Create(tso3);

            ////serviceBroker.Service.ServiceObjects.Create(wfo);
            ////serviceBroker.Service.ServiceObjects.Create(wfo2);
            ////serviceBroker.Service.ServiceObjects.Create(wfo3);


            ////ServiceFolder sf = new ServiceFolder("Templates", new MetaData("Templates","Folder for Template Service Objects"));
            ////serviceBroker.Service.ServiceFolders.Create(sf);
            ////sf.Add(tso);
            ////sf.Add(tso2);
            ////sf.Add(tso3);

            ////sf = null;
            ////sf = new ServiceFolder("Workflows", new MetaData("Workflows", "Folder for Workflow Service Objects"));
            ////serviceBroker.Service.ServiceFolders.Create(sf);
            ////sf.Add(wfo);
            ////sf.Add(wfo2);
            ////sf.Add(wfo3);

            ////sf = null;
            ////sf = new ServiceFolder("Documents", new MetaData("Documents", "Folder for Document Service Objects"));
            ////serviceBroker.Service.ServiceFolders.Create(sf);
            ////sf.Add(dso);


        }
        #endregion

        #region void SetTypeMappings()
        /// <summary>
        /// Sets the type mappings used to map the underlying data's types to the equivalent K2 SmartObject types.
        /// </summary>
        public void SetTypeMappings()
        {
            // Variable declaration.
            TypeMappings map = new TypeMappings();

            //TODO: Define Service Type mappings. These mappings are used to "convert" the internal data types 
            //of the Provider's properties to equivalent SmartObject types
            //In the sample code below, we are defining some common type mappings, but yours will probably differ
            map.Add("System.Int16", SoType.Number);
            map.Add("System.Int32", SoType.Number);
            map.Add("System.Int64", SoType.Number);
            map.Add("System.String", SoType.Text);
            map.Add("System.DateTime", SoType.DateTime);
            map.Add("System.Decimal", SoType.Decimal);
            map.Add("System.Guid", SoType.Guid);
            map.Add("System.Uri", SoType.HyperLink);
            map.Add("System.Xml.XmlDocument", SoType.Xml);
            map.Add("System.Boolean", SoType.YesNo); 

            // Add the type mappings to the service instance so that they can be retrieved easily
            serviceBroker.Service.ServiceConfiguration.Add(TYPEMAPPINGS, map);
        }
        #endregion

        #region TypeMappings GetTypeMappings()
        /// <summary>
        /// Gets the type mappings used to map the underlying data's types to the appropriate K2 SmartObject types.
        /// </summary>
        /// <returns>A TypeMappings object containing the ServiceBroker's type mappings which were previously stored in the service instance configuration.</returns>
        public TypeMappings GetTypeMappings()
        {
            // Lookup and return the type mappings stored in the service instance.
            return (TypeMappings)serviceBroker.Service.ServiceConfiguration[TYPEMAPPINGS];
        }
        #endregion

        #region void Execute(Property[] inputs, RequiredProperties required, Property[] returns, MethodType methodType, ServiceObject serviceObject)
        /// <summary>
        /// Entry point for all runtime method calls. Executes the Service Object method and returns any data.
        /// This method typically interrogates the requested method and then shells out to a helper method to perform the actual run-time processing
        /// </summary>
        /// <param name="inputProperties">A Property[] array containing all the defined input properties. Properties entered by the user will have a value.
        /// Properties not entered by the user will have a null value</param>
        /// <param name="requiredProperties">A collection of the NAMES of the Required properties for the method (note, not the values!)</param>
        /// <param name="returnProperties">A Property[] array containing all the defined return properties for the requested method</param>
        /// <param name="parameters">A collection of the Parameters defined for the method. Parameters are not included in the return Properties</param>
        /// <param name="methodType">A MethodType indicating what type of Service Object method was requested.</param>
        /// <param name="serviceObject">A ServiceObject definition containing populated properties for use with the method call.</param>
        public void Execute(Property[] inputProperties, RequiredProperties requiredProperties, Property[] returnProperties, MethodParameters parameters, MethodType methodType, ServiceObject serviceObject)
        {
            //TODO: Add runtime execution code here.
            //In a real-world implementation, You will need to interrogate the method type and method name to determine which method was called
            //and then perform the necessary operation in the back-end provider, passing in values from the inputs, required properties and parameters
            //the returnProperties indicates which properties in the serviceObject you should populate. 
            //You must populate the serviceObject with the results from the provider

            //OBTAINING THE SECURITY CREDENTIALS FOR A SERVICE INSTANCE AT RUNTIME
            //if you need to obtain the authentication credentials (username/password) for the service instance, query the following properties:
            //Note: password may be blank unless you are using Static or SSO credentials
            //string username = serviceBroker.Service.ServiceConfiguration.ServiceAuthentication.UserName;
            //string password = serviceBroker.Service.ServiceConfiguration.ServiceAuthentication.Password;

            //OBTAINING CONFIGURATION SETTINGS FOR A SERVICE AT RUNTIME
            //if you need to obtain configuration settings for the service instance and you did not define local members set by the GetConfiguraiotn() method, query the following property:
            //string configValue = serviceBroker.Service.ServiceConfiguration["ConfigItemName"].ToString();

            //in this sample, we are interrogating the method type and calling one of two helper methods, depending on whether this was a "Read" method or a "List" method
            //refer to the helper method to see how the various method parameters are used.
            //You will probably need to handle the other Method Types (e.g. Create, Delete etc.) as well.
            if (serviceObject.Name== "Documents ServiceObject")
            {
                switch (methodType)
                {
                    case MethodType.Read:
                        ExecuteDocumentRuntimeReadMethod(inputProperties, requiredProperties, returnProperties, parameters, serviceObject);
                        break;
                    case MethodType.List:
                        ExecuteDocumentRuntimeListMethod(inputProperties, requiredProperties, returnProperties, parameters, serviceObject);
                        break;
                    default:
                        throw new NotImplementedException("The helper for the specified method type (" + methodType.ToString() + ") has not been implemented");
                }
            }
            //switch (methodType)
            //{
            //    case MethodType.Read:
            //        ExecuteRuntimeReadMethod(inputProperties, requiredProperties, returnProperties, parameters, serviceObject);
            //        break;
            //    case MethodType.List:
            //        ExecuteRuntimeListMethod(inputProperties, requiredProperties, returnProperties, parameters, serviceObject);
            //        break;
            //    default:
            //        throw new NotImplementedException("The helper for the specified method type (" + methodType.ToString() + ") has not been implemented");
            //}
        }
        #endregion
 
        #region void Dispose()
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //TODO: Add any additional IDisposable implementation code here. Make sure to dispose of any data connections.

            // Clear reference to serviceBroker.
            serviceBroker = null;
        }
        #endregion

        #endregion

        #region Helper Methods (For demonstration purposes only)

        #region ServiceObject[] GenerateServiceObjects()
        /// <summary>
        /// Sample method which generates two Service Objects. You should replace this sample with your own code,
        /// but use the code below as a guide to understand how Service Objects are generated
        /// </summary>
        /// <returns>an Array of ServiceObjects</returns>
        public List<ServiceObject> GenerateServiceObjects()
        {
            //In this sample method, we are adding two Service Objects using an looping-approach to dynamically add Properties and Methods

            //generate a list to hold the service objects
            List<ServiceObject> sampleSvcObjects = new List<ServiceObject>();

            //variables to hold ServiceObject, Properties and Methods
            ServiceObject obj = null;
            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();

            //Generate two Service Objects. 
            //this looping-approach is for sample purposes only, to demonstrate adding a variable number of ServiceObjects
            //pay special attention to the Service Object generation
            for (int i = 1; i < 3; i++)
            {
                //Create a sample Service Object using the i counter to identify the object
                obj = new ServiceObject();
                //Note: ServiceObject Name should not contain space
                obj.Name = this.GetType().Namespace.Replace(" ", "") + ".Object" + i.ToString();
                string friendlyName = this.GetType().Assembly.GetName().Name + "." + this.GetType().Name;

                obj.MetaData.DisplayName = friendlyName + " ServiceObject " + i.ToString() + " Display Name";
                //IMPORTANT: You must activate the Service Object
                obj.Active = true;



                //Add Properties for the Service object. 
                //This loop is for sample purposes only to demonstrate adding a variable number of properties
                //pay attention to the Property setup and Property Type resolution
                for (int j = 1; j <= i + 2; j++)
                {
                    property = new Property();
                    property.Name = "Property" + j.ToString();
                    property.MetaData.DisplayName = " Property " + j.ToString() + " Display Name";
                    //randomly set the Property Type. If counter is even, use a String. If counter is odd, use a Number
                    //Property Type should be set to a .NET type
                    property.Type = j % 2 == 0 ? typeof(System.String).ToString() : typeof(System.Int32).ToString();
                    //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
                    property.SoType = map[property.Type];
                    //You can store the backend type here if you needed to, like this
                    //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
                    obj.Properties.Add(property);
                    property = null;
                }

                //Add Methods for the Service Object. 
                //This code is for example purposes only, to demonstrate adding a variable number of methods
                //notice the way we define a method, which Properties are available for input, which properties are required,
                //any parameters for the method and and which properties are returned by the method
                for (int k = 1; k <= i + 1; k++)
                {
                    //Add a Method for the Service Object
                    method = new Method();
                    method.Name = "Method" + k.ToString();
                    //randomly set the Method Type. If counter is even, use List if counter is odd, use Read
                    method.Type = k % 2 == 0 ? MethodType.List : MethodType.Read;
                    method.MetaData.DisplayName = " Method " + k.ToString() + " Display Name (" + method.Type.ToString() + ")";

                    //if the method type is Read, define a Key property using the first property for the ServiceObject
                    if (method.Type == MethodType.Read)
                    {
                        // Set up key property as the first Property of the SmartObject
                        method.InputProperties.Add(obj.Properties[0]);

                        // Mark the key property as required.
                        method.Validation.RequiredProperties.Add(obj.Properties[0]);

                        //Include a Method Parameter which is required, but not returned as a property
                        MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
                        parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
                        parm.MetaData.Description = "Sample Parameter";
                        method.MethodParameters.Create(parm);
                    }

                    //if the method type is List, add each of the available Properties as an available input property for the method
                    if (method.Type == MethodType.List)
                    {
                        foreach (Property prop in obj.Properties)
                        {
                            method.InputProperties.Add(prop);
                        }
                    }

                    //Set the method return Properties using all the available Properties of the ServiceObject
                    foreach (Property prop in obj.Properties)
                    {
                        method.ReturnProperties.Add(prop);
                    }

                    //add the method to the Service Object
                    obj.Methods.Add(method);
                    method = null;
                }

                //add the generated service object to the List of ServiceObjects
                sampleSvcObjects.Add(obj);
                obj = null;
            }

            //return the collection of defined Service Objects
            return sampleSvcObjects;

        }

        // The class needs to be designated as a ServiceObject and given a Name, DisplayName and Description.

        public ServiceObject GenerateDocumentServiceObject()
        {

            //document service object
            ServiceObject documentSvcObject = new ServiceObject();

            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();

            //Note: ServiceObject Name should not contain space
            documentSvcObject.Name = "Documents ServiceObject";
            documentSvcObject.MetaData.DisplayName = "Documents ServiceObject";
            //IMPORTANT: You must activate the Service Object
            documentSvcObject.Active = true;

            property = new Property();
            property.Name = "DocumentID";
            property.MetaData.DisplayName = "DocumentID";
            property.SoType = map[typeof(System.Int32).ToString()];
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "DocumentName";
            property.MetaData.DisplayName = "DocumentName";
            property.SoType = map[typeof(System.String).ToString()];
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "NumberOfPages";
            property.MetaData.DisplayName = "NumberOfPages";
            property.SoType = map[typeof(System.Int32).ToString()];
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "Path";
            property.MetaData.DisplayName = "Path";
            property.SoType = map[typeof(System.String).ToString()];
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "TemplateName";
            property.MetaData.DisplayName = "TemplateName";
            property.SoType = map[typeof(System.String).ToString()];
            documentSvcObject.Properties.Add(property);
            property = null;


            //Add a Method for the Service Object
            method = new Method();
            method.Name = "Get";
            method.Type = MethodType.Read;
            method.MetaData.DisplayName = "Get";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Read)
            {
                // Set up key property as the first Property of the SmartObject
                method.InputProperties.Add(documentSvcObject.Properties[0]);

                // Mark the key property as required.
                method.Validation.RequiredProperties.Add(documentSvcObject.Properties[0]);

                ////Include a Method Parameter which is required, but not returned as a property
                //MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
                //parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
                //parm.MetaData.Description = "Sample Parameter";
                //method.MethodParameters.Create(parm);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }

            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            //method = null;
            method = new Method();
            method.Name = "Insert";
            method.Type = MethodType.Update;
            method.MetaData.DisplayName = "Insert";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Update)
            {
                foreach (Property prop in documentSvcObject.Properties)
                {
                    method.InputProperties.Add(prop);
                }
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);

            //return the collection of defined Service Objects
            return documentSvcObject;



        }

        public ServiceObject GenerateTemplatesServiceObject()
        {

            //document service object
            ServiceObject documentSvcObject = new ServiceObject();

            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();


            //Note: ServiceObject Name should not contain space
            documentSvcObject.Name = "Templates1";
            documentSvcObject.MetaData.DisplayName = "Templates1";
            //IMPORTANT: You must activate the Service Object
            documentSvcObject.Active = true;

            property = new Property();
            property.Name = "DocumentID";
            property.MetaData.DisplayName = "DocumentID";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "DocumentName";
            property.MetaData.DisplayName = "DocumentName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "NumberOfPages";
            property.MetaData.DisplayName = "NumberOfPages";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "Path";
            property.MetaData.DisplayName = "Path";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "TemplateName";
            property.MetaData.DisplayName = "TemplateName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;


            //Add a Method for the Service Object
            method = new Method();
            method.Name = "Read";
            method.Type = MethodType.Read;
            method.MetaData.DisplayName = "Read";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Read)
            {
                // Set up key property as the first Property of the SmartObject
                method.InputProperties.Add(documentSvcObject.Properties[0]);

                // Mark the key property as required.
                method.Validation.RequiredProperties.Add(documentSvcObject.Properties[0]);

                ////Include a Method Parameter which is required, but not returned as a property
                //MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
                //parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
                //parm.MetaData.Description = "Sample Parameter";
                //method.MethodParameters.Create(parm);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }

            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);

            method = null;
            method = new Method();
            method.Name = "Write";
            method.Type = MethodType.Update;
            method.MetaData.DisplayName = "Write";
            //if the method type is Read, define a Key property using the first property for the ServiceObject

            // Set up key property as the first Property of the SmartObject
            method.InputProperties.Add(documentSvcObject.Properties[0]);

            // Mark the key property as required.
            method.Validation.RequiredProperties.Add(documentSvcObject.Properties[0]);

            ////Include a Method Parameter which is required, but not returned as a property
            //MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
            //parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
            //parm.MetaData.Description = "Sample Parameter";
            //method.MethodParameters.Create(parm);
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            method = null;
            method = new Method();
            method.Name = "Insert";
            method.Type = MethodType.Create;
            method.MetaData.DisplayName = "Insert";
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.InputProperties.Add(prop);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            method = null;
            method = new Method();
            method.Name = "Search";
            method.Type = MethodType.List;
            method.MetaData.DisplayName = "Search";
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.InputProperties.Add(prop);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);

            //return the collection of defined Service Objects
            return documentSvcObject;



        }

        public ServiceObject GenerateTemplatesServiceObject2()
        {

            //document service object
            ServiceObject documentSvcObject = new ServiceObject();

            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();


            //Note: ServiceObject Name should not contain space
            documentSvcObject.Name = "Templates2";
            documentSvcObject.MetaData.DisplayName = "Templates2";
            //IMPORTANT: You must activate the Service Object
            documentSvcObject.Active = true;

            property = new Property();
            property.Name = "DocumentID";
            property.MetaData.DisplayName = "DocumentID";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "DocumentName";
            property.MetaData.DisplayName = "DocumentName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "NumberOfPages";
            property.MetaData.DisplayName = "NumberOfPages";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "Path";
            property.MetaData.DisplayName = "Path";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "TemplateName";
            property.MetaData.DisplayName = "TemplateName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;


            //Add a Method for the Service Object
            method = new Method();
            method.Name = "Read";
            method.Type = MethodType.Read;
            method.MetaData.DisplayName = "Read";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Read)
            {
                // Set up key property as the first Property of the SmartObject
                method.InputProperties.Add(documentSvcObject.Properties[0]);

                // Mark the key property as required.
                method.Validation.RequiredProperties.Add(documentSvcObject.Properties[0]);

                ////Include a Method Parameter which is required, but not returned as a property
                //MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
                //parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
                //parm.MetaData.Description = "Sample Parameter";
                //method.MethodParameters.Create(parm);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }

            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);

            method = null;
            method = new Method();
            method.Name = "Write";
            method.Type = MethodType.Update;
            method.MetaData.DisplayName = "Write";
            //if the method type is Read, define a Key property using the first property for the ServiceObject

            // Set up key property as the first Property of the SmartObject
            method.InputProperties.Add(documentSvcObject.Properties[0]);

            // Mark the key property as required.
            method.Validation.RequiredProperties.Add(documentSvcObject.Properties[0]);

            ////Include a Method Parameter which is required, but not returned as a property
            //MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
            //parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
            //parm.MetaData.Description = "Sample Parameter";
            //method.MethodParameters.Create(parm);
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            method = null;
            method = new Method();
            method.Name = "Insert";
            method.Type = MethodType.Create;
            method.MetaData.DisplayName = "Insert";
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.InputProperties.Add(prop);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            method = null;
            method = new Method();
            method.Name = "Search";
            method.Type = MethodType.List;
            method.MetaData.DisplayName = "Search";
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.InputProperties.Add(prop);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);

            //return the collection of defined Service Objects
            return documentSvcObject;



        }

        public ServiceObject GenerateTemplatesServiceObject3()
        {

            //document service object
            ServiceObject documentSvcObject = new ServiceObject();

            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();


            //Note: ServiceObject Name should not contain space
            documentSvcObject.Name = "Templates3";
            documentSvcObject.MetaData.DisplayName = "Templates3";
            //IMPORTANT: You must activate the Service Object
            documentSvcObject.Active = true;

            property = new Property();
            property.Name = "DocumentID";
            property.MetaData.DisplayName = "DocumentID";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "DocumentName";
            property.MetaData.DisplayName = "DocumentName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "NumberOfPages";
            property.MetaData.DisplayName = "NumberOfPages";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "Path";
            property.MetaData.DisplayName = "Path";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "TemplateName";
            property.MetaData.DisplayName = "TemplateName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;


            //Add a Method for the Service Object
            method = new Method();
            method.Name = "Read";
            method.Type = MethodType.Read;
            method.MetaData.DisplayName = "Read";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Read)
            {
                // Set up key property as the first Property of the SmartObject
                method.InputProperties.Add(documentSvcObject.Properties[0]);

                // Mark the key property as required.
                method.Validation.RequiredProperties.Add(documentSvcObject.Properties[0]);

                ////Include a Method Parameter which is required, but not returned as a property
                //MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
                //parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
                //parm.MetaData.Description = "Sample Parameter";
                //method.MethodParameters.Create(parm);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }

            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);

            method = null;
            method = new Method();
            method.Name = "Write";
            method.Type = MethodType.Update;
            method.MetaData.DisplayName = "Write";
            //if the method type is Read, define a Key property using the first property for the ServiceObject

            // Set up key property as the first Property of the SmartObject
            method.InputProperties.Add(documentSvcObject.Properties[0]);

            // Mark the key property as required.
            method.Validation.RequiredProperties.Add(documentSvcObject.Properties[0]);

            ////Include a Method Parameter which is required, but not returned as a property
            //MethodParameter parm = new MethodParameter("Parameter " + k.ToString(), typeof(System.String).ToString(), SoType.Text, null);
            //parm.MetaData.DisplayName = "Parameter " + k.ToString() + " (Display Name)";
            //parm.MetaData.Description = "Sample Parameter";
            //method.MethodParameters.Create(parm);
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            method = null;
            method = new Method();
            method.Name = "Insert";
            method.Type = MethodType.Create;
            method.MetaData.DisplayName = "Insert";
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.InputProperties.Add(prop);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            method = null;
            method = new Method();
            method.Name = "Search";
            method.Type = MethodType.List;
            method.MetaData.DisplayName = "Search";
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.InputProperties.Add(prop);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }
            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);

            //return the collection of defined Service Objects
            return documentSvcObject;



        }

        public ServiceObject GenerateWorkflowsServiceObject()
        {

            //document service object
            ServiceObject documentSvcObject = new ServiceObject();

            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();


            //Note: ServiceObject Name should not contain space
            documentSvcObject.Name = "Workflows1";
            documentSvcObject.MetaData.DisplayName = "Workflows1";
            //IMPORTANT: You must activate the Service Object
            documentSvcObject.Active = true;

            property = new Property();
            property.Name = "WorkflowID";
            property.MetaData.DisplayName = "WorkflowID";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "WorkflowName";
            property.MetaData.DisplayName = "WorkflowName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            //Add a Method for the Service Object
            method = new Method();
            method.Name = "Start";
            method.Type = MethodType.Execute;
            method.MetaData.DisplayName = "Start";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Execute)
            {
                foreach (Property prop in documentSvcObject.Properties)
                {
                    method.InputProperties.Add(prop);
                }

                //Include a Method Parameter, not returned as a property
                MethodParameter parm = new MethodParameter("EntryID", typeof(System.Int32).ToString(), SoType.Number, null);
                parm.IsRequired = false;
                parm.MetaData.DisplayName = "EntryID";
                parm.MetaData.Description = "EntryID";
                method.MethodParameters.Create(parm);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }

            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            //return the collection of defined Service Objects
            return documentSvcObject;

        }

        public ServiceObject GenerateWorkflowsServiceObject2()
        {

            //document service object
            ServiceObject documentSvcObject = new ServiceObject();

            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();


            //Note: ServiceObject Name should not contain space
            documentSvcObject.Name = "Workflows2";
            documentSvcObject.MetaData.DisplayName = "Workflows2";
            //IMPORTANT: You must activate the Service Object
            documentSvcObject.Active = true;

            property = new Property();
            property.Name = "WorkflowID";
            property.MetaData.DisplayName = "WorkflowID";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "WorkflowName";
            property.MetaData.DisplayName = "WorkflowName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            //Add a Method for the Service Object
            method = new Method();
            method.Name = "Start";
            method.Type = MethodType.Execute;
            method.MetaData.DisplayName = "Start";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Execute)
            {
                foreach (Property prop in documentSvcObject.Properties)
                {
                    method.InputProperties.Add(prop);
                }

                //Include a Method Parameter, not returned as a property
                MethodParameter parm = new MethodParameter("EntryID", typeof(System.Int32).ToString(), SoType.Number, null);
                parm.IsRequired = false;
                parm.MetaData.DisplayName = "EntryID";
                parm.MetaData.Description = "EntryID";
                method.MethodParameters.Create(parm);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }

            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            //return the collection of defined Service Objects
            return documentSvcObject;

        }

        public ServiceObject GenerateWorkflowsServiceObject3()
        {

            //document service object
            ServiceObject documentSvcObject = new ServiceObject();

            Property property = null;
            Method method = null;
            //load the Type Mappings as defined by the SetTypeMappings() method
            TypeMappings map = GetTypeMappings();


            //Note: ServiceObject Name should not contain space
            documentSvcObject.Name = "Workflows3";
            documentSvcObject.MetaData.DisplayName = "Workflows3";
            //IMPORTANT: You must activate the Service Object
            documentSvcObject.Active = true;

            property = new Property();
            property.Name = "WorkflowID";
            property.MetaData.DisplayName = "WorkflowID";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.Int32).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            property = new Property();
            property.Name = "WorkflowName";
            property.MetaData.DisplayName = "WorkflowName";
            //Property Type should be set to a .NET type
            //based on the Property type, lookup and set SoType using the Property Mappings defined in the SetTypeMappings() method
            property.SoType = map[typeof(System.String).ToString()];
            //You can store the backend type here if you needed to, like this
            //property.MetaData.ServiceProperties.Add("String", "TEXT"); 
            documentSvcObject.Properties.Add(property);
            property = null;

            //Add a Method for the Service Object
            method = new Method();
            method.Name = "Start";
            method.Type = MethodType.Execute;
            method.MetaData.DisplayName = "Start";
            //if the method type is Read, define a Key property using the first property for the ServiceObject
            if (method.Type == MethodType.Execute)
            {
                foreach (Property prop in documentSvcObject.Properties)
                {
                    method.InputProperties.Add(prop);
                }

                //Include a Method Parameter, not returned as a property
                MethodParameter parm = new MethodParameter("EntryID", typeof(System.Int32).ToString(), SoType.Number, null);
                parm.IsRequired = false;
                parm.MetaData.DisplayName = "EntryID";
                parm.MetaData.Description = "EntryID";
                method.MethodParameters.Create(parm);
            }
            //Set the method return Properties using all the available Properties of the ServiceObject
            foreach (Property prop in documentSvcObject.Properties)
            {
                method.ReturnProperties.Add(prop);
            }

            //add the method to the Service Object
            documentSvcObject.Methods.Add(method);


            //return the collection of defined Service Objects
            return documentSvcObject;

        }

        #endregion

        /// <summary>
        /// This is a sample method to demonstrate a Read operation at runtime
        /// </summary>
        /// <param name="inputProperties"></param>
        /// <param name="requiredProperties"></param>
        /// <param name="returnProperties"></param>
        /// <param name="parameters"></param>
        /// <param name="serviceObject"></param>
        private void ExecuteDocumentRuntimeReadMethod(Property[] inputProperties, RequiredProperties requiredProperties, Property[] returnProperties, MethodParameters parameters, ServiceObject serviceObject)
        {
            //Prepare the Service Object to receive returned data.
            serviceObject.Properties.InitResultTable();

            //If you needed to, you can access method parameters like this:
            foreach (MethodParameter parameter in parameters)
            {
                string parameterVame = parameter.Name;
                object parameterValue = parameter.Value;
            }

            LaserficheProvider lp = new LaserficheProvider(this._requiredLaserficheServerValue,this._requiredLaserficheRepositoryValue);
            DocumentInfo docInfo;
            lp.Connect();
            docInfo = lp.GetDocumentByEntryID(Int32.Parse(inputProperties[0].Value.ToString()));

            //For demonstration purposes, set the Service Object (record)'s properties with random values.
            Random rnd = new Random();
            for (int i = 0; i < returnProperties.Length; i++)
            {
                //if this is the first (key) value, set the property to the first input property to simulate a read record based on some key input property
                if (returnProperties[i].Name == requiredProperties[0])
                {
                    serviceObject.Properties[i].Value = inputProperties[i].Value;
                }
                //for all other properties, just use random values
                else
                    switch (serviceObject.Properties[i].SoType)
                    {
                        case SoType.Text:
                            serviceObject.Properties[i].Value = RandomString(rnd.Next(8, 20));
                            break;
                        case SoType.Number:
                            serviceObject.Properties[i].Value = rnd.Next(5, 100);
                            break;
                    }
            }

            //Commit the changes to the Service Object. 
            serviceObject.Properties.BindPropertiesToResultTable();
        }
        private void ExecuteRuntimeReadMethod(Property[] inputProperties, RequiredProperties requiredProperties, Property[] returnProperties, MethodParameters parameters,  ServiceObject serviceObject)
        {
            //Prepare the Service Object to receive returned data.
            serviceObject.Properties.InitResultTable();

            //If you needed to, you can access method parameters like this:
            foreach (MethodParameter parameter in parameters)
            {
                string parameterVame = parameter.Name;
                object parameterValue = parameter.Value; 
            }

            //For demonstration purposes, set the Service Object (record)'s properties with random values.
            Random rnd = new Random();
            for (int i = 0; i < returnProperties.Length; i++)
            {
                //if this is the first (key) value, set the property to the first input property to simulate a read record based on some key input property
                if (returnProperties[i].Name == requiredProperties[0])
                {
                    serviceObject.Properties[i].Value = inputProperties[i].Value;
                }
                //for all other properties, just use random values
                else
                    switch (serviceObject.Properties[i].SoType)
                    {
                        case SoType.Text:
                            serviceObject.Properties[i].Value = RandomString(rnd.Next(8, 20));
                            break;
                        case SoType.Number:
                            serviceObject.Properties[i].Value = rnd.Next(5, 100);
                            break;
                    }
            }

            //Commit the changes to the Service Object. 
            serviceObject.Properties.BindPropertiesToResultTable();
        }

        /// <summary>
        /// This is a sample method to demonstrate a List operation at runtime
        /// </summary>
        /// <param name="inputProperties"></param>
        /// <param name="requiredProperties"></param>
        /// <param name="returnProperties"></param>
        /// <param name="parameters"></param>
        /// <param name="serviceObject"></param>
        private void ExecuteDocumentRuntimeListMethod(Property[] inputProperties, RequiredProperties requiredProperties, Property[] returnProperties, MethodParameters parameters, ServiceObject serviceObject)
        {

            //store the input properties in a temp location since they will be overwritten when BindPropertiesToResultTable() is called
            System.Collections.Specialized.NameValueCollection tempInputProperties = new System.Collections.Specialized.NameValueCollection();
            foreach (Property prop in inputProperties)
            {
                if (prop.Value != null)
                {
                    tempInputProperties.Add(prop.Name, prop.Value.ToString());
                }
            }

            //If you needed to, you can access method parameters like this:
            foreach (MethodParameter parameter in parameters)
            {
                string parameterVame = parameter.Name;
                object parameterValue = parameter.Value;
            }

            // Prepare the Service Object to receive returned data.
            serviceObject.Properties.InitResultTable();

            //return a random number of records
            Random rnd = new Random();
            for (int j = 0; j < rnd.Next(3, 7); j++)
            {
                // Set Service Object (record)'s properties
                for (int i = 0; i < returnProperties.Length; i++)
                {
                    //if input properties were provided, use those...
                    if (tempInputProperties[serviceObject.Properties[i].Name] != null)
                    {
                        serviceObject.Properties[i].Value = tempInputProperties[serviceObject.Properties[i].Name];
                    }
                    //otherwise just populate the properties with random values
                    else
                        switch (serviceObject.Properties[i].SoType)
                        {
                            case SoType.Text:
                                serviceObject.Properties[i].Value = RandomString(rnd.Next(8, 20));
                                break;
                            case SoType.Number:
                                serviceObject.Properties[i].Value = rnd.Next(5, 100);
                                break;
                        }
                }
                // Commit the changes to the Service Object for each record returned by the List method
                //Note that this will update the inputProperties collection as well
                serviceObject.Properties.BindPropertiesToResultTable();
            }
        }
        private void ExecuteRuntimeListMethod(Property[] inputProperties, RequiredProperties requiredProperties, Property[] returnProperties, MethodParameters parameters, ServiceObject serviceObject)
        {

            //store the input properties in a temp location since they will be overwritten when BindPropertiesToResultTable() is called
            System.Collections.Specialized.NameValueCollection tempInputProperties = new System.Collections.Specialized.NameValueCollection();
            foreach (Property prop in inputProperties)
            {
                if (prop.Value != null)
                {
                    tempInputProperties.Add(prop.Name, prop.Value.ToString());
                }
            }

            //If you needed to, you can access method parameters like this:
            foreach (MethodParameter parameter in parameters)
            {
                string parameterVame = parameter.Name;
                object parameterValue = parameter.Value;
            }

            // Prepare the Service Object to receive returned data.
            serviceObject.Properties.InitResultTable();

            //return a random number of records
            Random rnd = new Random();
            for (int j = 0; j < rnd.Next(3, 7); j++)
            {
                // Set Service Object (record)'s properties
                for (int i = 0; i < returnProperties.Length; i++)
                {
                    //if input properties were provided, use those...
                    if (tempInputProperties[serviceObject.Properties[i].Name] != null)
                    {
                        serviceObject.Properties[i].Value = tempInputProperties[serviceObject.Properties[i].Name];
                    }
                    //otherwise just populate the properties with random values
                    else
                        switch (serviceObject.Properties[i].SoType)
                        {
                            case SoType.Text:
                                serviceObject.Properties[i].Value = RandomString(rnd.Next(8, 20));
                                break;
                            case SoType.Number:
                                serviceObject.Properties[i].Value = rnd.Next(5, 100);
                                break;
                        }
                }
                // Commit the changes to the Service Object for each record returned by the List method
                //Note that this will update the inputProperties collection as well
                serviceObject.Properties.BindPropertiesToResultTable();
            }
        }


        /// <summary>
        /// private helper method used to generate a random string. used for sample purposes only.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        #endregion
    }
}