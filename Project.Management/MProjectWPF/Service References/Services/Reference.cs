﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MProjectWPF.Services {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Services.IServices")]
    public interface IServices {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServices/sendEmail", ReplyAction="http://tempuri.org/IServices/sendEmailResponse")]
        string sendEmail(string emaildes, string subject, string mensaje);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServices/sendEmail", ReplyAction="http://tempuri.org/IServices/sendEmailResponse")]
        System.Threading.Tasks.Task<string> sendEmailAsync(string emaildes, string subject, string mensaje);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicesChannel : MProjectWPF.Services.IServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicesClient : System.ServiceModel.ClientBase<MProjectWPF.Services.IServices>, MProjectWPF.Services.IServices {
        
        public ServicesClient() {
        }
        
        public ServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string sendEmail(string emaildes, string subject, string mensaje) {
            return base.Channel.sendEmail(emaildes, subject, mensaje);
        }
        
        public System.Threading.Tasks.Task<string> sendEmailAsync(string emaildes, string subject, string mensaje) {
            return base.Channel.sendEmailAsync(emaildes, subject, mensaje);
        }
    }
}
