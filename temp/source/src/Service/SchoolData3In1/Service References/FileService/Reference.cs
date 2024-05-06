﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolData3In1.FileService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FileService.FileServiceSoap")]
    public interface FileServiceSoap {
        
        // CODEGEN: 命名空間 http://tempuri.org/ 的元素名稱  apData 未標示為 nillable，正在產生訊息合約
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FTPUpload", ReplyAction="*")]
        SchoolData3In1.FileService.FTPUploadResponse FTPUpload(SchoolData3In1.FileService.FTPUploadRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FTPUpload", ReplyAction="*")]
        System.Threading.Tasks.Task<SchoolData3In1.FileService.FTPUploadResponse> FTPUploadAsync(SchoolData3In1.FileService.FTPUploadRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class FTPUploadRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="FTPUpload", Namespace="http://tempuri.org/", Order=0)]
        public SchoolData3In1.FileService.FTPUploadRequestBody Body;
        
        public FTPUploadRequest() {
        }
        
        public FTPUploadRequest(SchoolData3In1.FileService.FTPUploadRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class FTPUploadRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string apData;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int jobNo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string jobTypeId;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string fileKind;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public byte[] zipContents;
        
        public FTPUploadRequestBody() {
        }
        
        public FTPUploadRequestBody(string apData, int jobNo, string jobTypeId, string fileKind, byte[] zipContents) {
            this.apData = apData;
            this.jobNo = jobNo;
            this.jobTypeId = jobTypeId;
            this.fileKind = fileKind;
            this.zipContents = zipContents;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class FTPUploadResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="FTPUploadResponse", Namespace="http://tempuri.org/", Order=0)]
        public SchoolData3In1.FileService.FTPUploadResponseBody Body;
        
        public FTPUploadResponse() {
        }
        
        public FTPUploadResponse(SchoolData3In1.FileService.FTPUploadResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class FTPUploadResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string FTPUploadResult;
        
        public FTPUploadResponseBody() {
        }
        
        public FTPUploadResponseBody(string FTPUploadResult) {
            this.FTPUploadResult = FTPUploadResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface FileServiceSoapChannel : SchoolData3In1.FileService.FileServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileServiceSoapClient : System.ServiceModel.ClientBase<SchoolData3In1.FileService.FileServiceSoap>, SchoolData3In1.FileService.FileServiceSoap {
        
        public FileServiceSoapClient() {
        }
        
        public FileServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SchoolData3In1.FileService.FTPUploadResponse SchoolData3In1.FileService.FileServiceSoap.FTPUpload(SchoolData3In1.FileService.FTPUploadRequest request) {
            return base.Channel.FTPUpload(request);
        }
        
        public string FTPUpload(string apData, int jobNo, string jobTypeId, string fileKind, byte[] zipContents) {
            SchoolData3In1.FileService.FTPUploadRequest inValue = new SchoolData3In1.FileService.FTPUploadRequest();
            inValue.Body = new SchoolData3In1.FileService.FTPUploadRequestBody();
            inValue.Body.apData = apData;
            inValue.Body.jobNo = jobNo;
            inValue.Body.jobTypeId = jobTypeId;
            inValue.Body.fileKind = fileKind;
            inValue.Body.zipContents = zipContents;
            SchoolData3In1.FileService.FTPUploadResponse retVal = ((SchoolData3In1.FileService.FileServiceSoap)(this)).FTPUpload(inValue);
            return retVal.Body.FTPUploadResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SchoolData3In1.FileService.FTPUploadResponse> SchoolData3In1.FileService.FileServiceSoap.FTPUploadAsync(SchoolData3In1.FileService.FTPUploadRequest request) {
            return base.Channel.FTPUploadAsync(request);
        }
        
        public System.Threading.Tasks.Task<SchoolData3In1.FileService.FTPUploadResponse> FTPUploadAsync(string apData, int jobNo, string jobTypeId, string fileKind, byte[] zipContents) {
            SchoolData3In1.FileService.FTPUploadRequest inValue = new SchoolData3In1.FileService.FTPUploadRequest();
            inValue.Body = new SchoolData3In1.FileService.FTPUploadRequestBody();
            inValue.Body.apData = apData;
            inValue.Body.jobNo = jobNo;
            inValue.Body.jobTypeId = jobTypeId;
            inValue.Body.fileKind = fileKind;
            inValue.Body.zipContents = zipContents;
            return ((SchoolData3In1.FileService.FileServiceSoap)(this)).FTPUploadAsync(inValue);
        }
    }
}
