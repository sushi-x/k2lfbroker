using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SourceCode.SmartObjects.Services.ServiceSDK.Attributes;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace LaserficheServiceObject.Interfaces
{
    /// <summary>
    /// An interface for concrete classes responsible for interacting with underlying systems or technologies.
    /// You do not need to make any changes to this class
    /// </summary>
    interface IDataConnector : IDisposable
    {
        #region Methods

        #region void GetConfiguration()
        /// <summary>
        /// Gets the configuration from the service instance and stores the retrieved configuration in local variables for later use.
        /// </summary>
        void GetConfiguration();
        #endregion

        #region void SetupConfiguration()
        /// <summary>
        /// Sets up the required configuration parameters in the service instance. When a new service instance is registered for this ServiceBroker, the configuration parameters are surfaced to the appropriate tooling. The configuration parameters are provided by the person registering the service instance.
        /// </summary>
        void SetupConfiguration();
        #endregion

        #region void SetupService()
        /// <summary>
        /// Sets up the service instance's default name, display name, and description.
        /// </summary>
        void SetupService();
        #endregion

        #region void DescribeSchema()
        /// <summary>
        /// Describes the schema of the underlying data and services to the K2 platform.
        /// </summary>
        void DescribeSchema();
        #endregion

        #region TypeMappings GetTypeMappings()
        /// <summary>
        /// Gets the type mappings used to map the underlying data's types to the appropriate K2 SmartObject types.
        /// </summary>
        /// <returns>A TypeMappings object containing the ServiceBroker's type mappings which were previously stored in the service instance configuration.</returns>
        TypeMappings GetTypeMappings();
        #endregion

        #region void SetTypeMappings()
        /// <summary>
        /// Sets the type mappings used to map the underlying data's types to the appropriate K2 SmartObject types.
        /// </summary>
        void SetTypeMappings();
        #endregion

        #region void Execute(Property[] inputProperties, RequiredProperties required, CompositeProperty[] returnProperties, MethodType methodType, ServiceObject serviceObject)
        /// <summary>
        /// Executes the Service Object method and returns any data.
        /// </summary>
        /// <param name="inputProperties">A Property[] array containing all the allowed input properties.</param>
        /// <param name="requiredProperties">A RequiredProperties collection containing the required properties.</param>
        /// <param name="returnProperties">A Property[] array containing all the allowed return properties.</param>
        /// <param name="parameters">A collection of the Parameters defined for the method. Parameters are not included in the return Properties</param>
        /// <param name="methodType">A MethodType indicating what type of Service Object method was called.</param>
        /// <param name="serviceObject">A ServiceObject containing populated properties for use with the method call.</param>
        void Execute(Property[] inputProperties, RequiredProperties requiredProperties, Property[] returnProperties, MethodParameters parameters, MethodType methodType, ServiceObject serviceObject);
        #endregion

        #endregion
    }
}