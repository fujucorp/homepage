﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entities.MsgHandler {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.cedar.com.tw/bluestar/", ConfigurationName="MsgHandler.MsgHandlerSoap")]
    public interface MsgHandlerSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/SubmitXmlSync", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Entities.MsgHandler.SubmitXmlSyncResponse SubmitXmlSync(Entities.MsgHandler.SubmitXmlSyncRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/SubmitXmlSync", ReplyAction="*")]
        System.Threading.Tasks.Task<Entities.MsgHandler.SubmitXmlSyncResponse> SubmitXmlSyncAsync(Entities.MsgHandler.SubmitXmlSyncRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/submitXml", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Entities.MsgHandler.SubmitXmlResponse SubmitXml(Entities.MsgHandler.SubmitXmlRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/submitXml", ReplyAction="*")]
        System.Threading.Tasks.Task<Entities.MsgHandler.SubmitXmlResponse> SubmitXmlAsync(Entities.MsgHandler.SubmitXmlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/SubmitXmlString", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Entities.MsgHandler.SubmitXmlStringResponse SubmitXmlString(Entities.MsgHandler.SubmitXmlStringRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/SubmitXmlString", ReplyAction="*")]
        System.Threading.Tasks.Task<Entities.MsgHandler.SubmitXmlStringResponse> SubmitXmlStringAsync(Entities.MsgHandler.SubmitXmlStringRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/submitFlatFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Entities.MsgHandler.SubmitFlatFileResponse SubmitFlatFile(Entities.MsgHandler.SubmitFlatFileRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/submitFlatFile", ReplyAction="*")]
        System.Threading.Tasks.Task<Entities.MsgHandler.SubmitFlatFileResponse> SubmitFlatFileAsync(Entities.MsgHandler.SubmitFlatFileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/FlatFile2XmlString", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Entities.MsgHandler.FlatFile2XmlStringResponse FlatFile2XmlString(Entities.MsgHandler.FlatFile2XmlStringRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/FlatFile2XmlString", ReplyAction="*")]
        System.Threading.Tasks.Task<Entities.MsgHandler.FlatFile2XmlStringResponse> FlatFile2XmlStringAsync(Entities.MsgHandler.FlatFile2XmlStringRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/FlatFile2XmlElement", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Entities.MsgHandler.FlatFile2XmlElementResponse FlatFile2XmlElement(Entities.MsgHandler.FlatFile2XmlElementRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/FlatFile2XmlElement", ReplyAction="*")]
        System.Threading.Tasks.Task<Entities.MsgHandler.FlatFile2XmlElementResponse> FlatFile2XmlElementAsync(Entities.MsgHandler.FlatFile2XmlElementRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/XmlStringToFlatFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Entities.MsgHandler.XmlString2FlatFileResponse XmlString2FlatFile(Entities.MsgHandler.XmlString2FlatFileRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://www.cedar.com.tw/bluestar/XmlStringToFlatFile", ReplyAction="*")]
        System.Threading.Tasks.Task<Entities.MsgHandler.XmlString2FlatFileResponse> XmlString2FlatFileAsync(Entities.MsgHandler.XmlString2FlatFileRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitXmlSync", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitXmlSyncRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement Any;
        
        public SubmitXmlSyncRequest() {
        }
        
        public SubmitXmlSyncRequest(System.Xml.XmlElement Any) {
            this.Any = Any;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitXmlSyncResponse", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitXmlSyncResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement Any;
        
        public SubmitXmlSyncResponse() {
        }
        
        public SubmitXmlSyncResponse(System.Xml.XmlElement Any) {
            this.Any = Any;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitXml", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitXmlRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement Any;
        
        public SubmitXmlRequest() {
        }
        
        public SubmitXmlRequest(System.Xml.XmlElement Any) {
            this.Any = Any;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitXmlResponse", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitXmlResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public int SubmitXmlResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public System.Xml.XmlElement response;
        
        public SubmitXmlResponse() {
        }
        
        public SubmitXmlResponse(int SubmitXmlResult, System.Xml.XmlElement response) {
            this.SubmitXmlResult = SubmitXmlResult;
            this.response = response;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitXmlStringRequest", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitXmlStringRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public string sRequest;
        
        public SubmitXmlStringRequest() {
        }
        
        public SubmitXmlStringRequest(string sRequest) {
            this.sRequest = sRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitXmlStringResponse", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitXmlStringResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public int SubmitXmlStringResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public string sResponse;
        
        public SubmitXmlStringResponse() {
        }
        
        public SubmitXmlStringResponse(int SubmitXmlStringResult, string sResponse) {
            this.SubmitXmlStringResult = SubmitXmlStringResult;
            this.sResponse = sResponse;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitFlatFile", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitFlatFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public string msgName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public string request;
        
        public SubmitFlatFileRequest() {
        }
        
        public SubmitFlatFileRequest(string msgName, string request) {
            this.msgName = msgName;
            this.request = request;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SubmitFlatFileResponse", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class SubmitFlatFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public int SubmitFlatFileResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public string response;
        
        public SubmitFlatFileResponse() {
        }
        
        public SubmitFlatFileResponse(int SubmitFlatFileResult, string response) {
            this.SubmitFlatFileResult = SubmitFlatFileResult;
            this.response = response;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FlatFile2XmlString", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class FlatFile2XmlStringRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public string MsgName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public string Tota;
        
        public FlatFile2XmlStringRequest() {
        }
        
        public FlatFile2XmlStringRequest(string MsgName, string Tota) {
            this.MsgName = MsgName;
            this.Tota = Tota;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FlatFile2XmlStringResponse", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class FlatFile2XmlStringResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public int FlatFile2XmlStringResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public string XmlString;
        
        public FlatFile2XmlStringResponse() {
        }
        
        public FlatFile2XmlStringResponse(int FlatFile2XmlStringResult, string XmlString) {
            this.FlatFile2XmlStringResult = FlatFile2XmlStringResult;
            this.XmlString = XmlString;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FlatFile2XmlElement", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class FlatFile2XmlElementRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public string MsgName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public string Tota;
        
        public FlatFile2XmlElementRequest() {
        }
        
        public FlatFile2XmlElementRequest(string MsgName, string Tota) {
            this.MsgName = MsgName;
            this.Tota = Tota;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FlatFile2XmlElementResponse", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class FlatFile2XmlElementResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public int FlatFile2XmlElementResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public System.Xml.XmlElement RequestXmlElement;
        
        public FlatFile2XmlElementResponse() {
        }
        
        public FlatFile2XmlElementResponse(int FlatFile2XmlElementResult, System.Xml.XmlElement RequestXmlElement) {
            this.FlatFile2XmlElementResult = FlatFile2XmlElementResult;
            this.RequestXmlElement = RequestXmlElement;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="XmlString2FlatFile", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class XmlString2FlatFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public string xmlRequest;
        
        public XmlString2FlatFileRequest() {
        }
        
        public XmlString2FlatFileRequest(string xmlRequest) {
            this.xmlRequest = xmlRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="XmlString2FlatFileResponse", WrapperNamespace="http://www.cedar.com.tw/bluestar/", IsWrapped=true)]
    public partial class XmlString2FlatFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=0)]
        public int XmlString2FlatFileResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.cedar.com.tw/bluestar/", Order=1)]
        public string tita;
        
        public XmlString2FlatFileResponse() {
        }
        
        public XmlString2FlatFileResponse(int XmlString2FlatFileResult, string tita) {
            this.XmlString2FlatFileResult = XmlString2FlatFileResult;
            this.tita = tita;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MsgHandlerSoapChannel : Entities.MsgHandler.MsgHandlerSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MsgHandlerSoapClient : System.ServiceModel.ClientBase<Entities.MsgHandler.MsgHandlerSoap>, Entities.MsgHandler.MsgHandlerSoap {
        
        public MsgHandlerSoapClient() {
        }
        
        public MsgHandlerSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MsgHandlerSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MsgHandlerSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MsgHandlerSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Entities.MsgHandler.SubmitXmlSyncResponse Entities.MsgHandler.MsgHandlerSoap.SubmitXmlSync(Entities.MsgHandler.SubmitXmlSyncRequest request) {
            return base.Channel.SubmitXmlSync(request);
        }
        
        public void SubmitXmlSync(ref System.Xml.XmlElement Any) {
            Entities.MsgHandler.SubmitXmlSyncRequest inValue = new Entities.MsgHandler.SubmitXmlSyncRequest();
            inValue.Any = Any;
            Entities.MsgHandler.SubmitXmlSyncResponse retVal = ((Entities.MsgHandler.MsgHandlerSoap)(this)).SubmitXmlSync(inValue);
            Any = retVal.Any;
        }
        
        public System.Threading.Tasks.Task<Entities.MsgHandler.SubmitXmlSyncResponse> SubmitXmlSyncAsync(Entities.MsgHandler.SubmitXmlSyncRequest request) {
            return base.Channel.SubmitXmlSyncAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Entities.MsgHandler.SubmitXmlResponse Entities.MsgHandler.MsgHandlerSoap.SubmitXml(Entities.MsgHandler.SubmitXmlRequest request) {
            return base.Channel.SubmitXml(request);
        }
        
        public int SubmitXml(System.Xml.XmlElement Any, out System.Xml.XmlElement response) {
            Entities.MsgHandler.SubmitXmlRequest inValue = new Entities.MsgHandler.SubmitXmlRequest();
            inValue.Any = Any;
            Entities.MsgHandler.SubmitXmlResponse retVal = ((Entities.MsgHandler.MsgHandlerSoap)(this)).SubmitXml(inValue);
            response = retVal.response;
            return retVal.SubmitXmlResult;
        }
        
        public System.Threading.Tasks.Task<Entities.MsgHandler.SubmitXmlResponse> SubmitXmlAsync(Entities.MsgHandler.SubmitXmlRequest request) {
            return base.Channel.SubmitXmlAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Entities.MsgHandler.SubmitXmlStringResponse Entities.MsgHandler.MsgHandlerSoap.SubmitXmlString(Entities.MsgHandler.SubmitXmlStringRequest request) {
            return base.Channel.SubmitXmlString(request);
        }
        
        public int SubmitXmlString(string sRequest, out string sResponse) {
            Entities.MsgHandler.SubmitXmlStringRequest inValue = new Entities.MsgHandler.SubmitXmlStringRequest();
            inValue.sRequest = sRequest;
            Entities.MsgHandler.SubmitXmlStringResponse retVal = ((Entities.MsgHandler.MsgHandlerSoap)(this)).SubmitXmlString(inValue);
            sResponse = retVal.sResponse;
            return retVal.SubmitXmlStringResult;
        }
        
        public System.Threading.Tasks.Task<Entities.MsgHandler.SubmitXmlStringResponse> SubmitXmlStringAsync(Entities.MsgHandler.SubmitXmlStringRequest request) {
            return base.Channel.SubmitXmlStringAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Entities.MsgHandler.SubmitFlatFileResponse Entities.MsgHandler.MsgHandlerSoap.SubmitFlatFile(Entities.MsgHandler.SubmitFlatFileRequest request) {
            return base.Channel.SubmitFlatFile(request);
        }
        
        public int SubmitFlatFile(string msgName, string request, out string response) {
            Entities.MsgHandler.SubmitFlatFileRequest inValue = new Entities.MsgHandler.SubmitFlatFileRequest();
            inValue.msgName = msgName;
            inValue.request = request;
            Entities.MsgHandler.SubmitFlatFileResponse retVal = ((Entities.MsgHandler.MsgHandlerSoap)(this)).SubmitFlatFile(inValue);
            response = retVal.response;
            return retVal.SubmitFlatFileResult;
        }
        
        public System.Threading.Tasks.Task<Entities.MsgHandler.SubmitFlatFileResponse> SubmitFlatFileAsync(Entities.MsgHandler.SubmitFlatFileRequest request) {
            return base.Channel.SubmitFlatFileAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Entities.MsgHandler.FlatFile2XmlStringResponse Entities.MsgHandler.MsgHandlerSoap.FlatFile2XmlString(Entities.MsgHandler.FlatFile2XmlStringRequest request) {
            return base.Channel.FlatFile2XmlString(request);
        }
        
        public int FlatFile2XmlString(string MsgName, string Tota, out string XmlString) {
            Entities.MsgHandler.FlatFile2XmlStringRequest inValue = new Entities.MsgHandler.FlatFile2XmlStringRequest();
            inValue.MsgName = MsgName;
            inValue.Tota = Tota;
            Entities.MsgHandler.FlatFile2XmlStringResponse retVal = ((Entities.MsgHandler.MsgHandlerSoap)(this)).FlatFile2XmlString(inValue);
            XmlString = retVal.XmlString;
            return retVal.FlatFile2XmlStringResult;
        }
        
        public System.Threading.Tasks.Task<Entities.MsgHandler.FlatFile2XmlStringResponse> FlatFile2XmlStringAsync(Entities.MsgHandler.FlatFile2XmlStringRequest request) {
            return base.Channel.FlatFile2XmlStringAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Entities.MsgHandler.FlatFile2XmlElementResponse Entities.MsgHandler.MsgHandlerSoap.FlatFile2XmlElement(Entities.MsgHandler.FlatFile2XmlElementRequest request) {
            return base.Channel.FlatFile2XmlElement(request);
        }
        
        public int FlatFile2XmlElement(string MsgName, string Tota, out System.Xml.XmlElement RequestXmlElement) {
            Entities.MsgHandler.FlatFile2XmlElementRequest inValue = new Entities.MsgHandler.FlatFile2XmlElementRequest();
            inValue.MsgName = MsgName;
            inValue.Tota = Tota;
            Entities.MsgHandler.FlatFile2XmlElementResponse retVal = ((Entities.MsgHandler.MsgHandlerSoap)(this)).FlatFile2XmlElement(inValue);
            RequestXmlElement = retVal.RequestXmlElement;
            return retVal.FlatFile2XmlElementResult;
        }
        
        public System.Threading.Tasks.Task<Entities.MsgHandler.FlatFile2XmlElementResponse> FlatFile2XmlElementAsync(Entities.MsgHandler.FlatFile2XmlElementRequest request) {
            return base.Channel.FlatFile2XmlElementAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Entities.MsgHandler.XmlString2FlatFileResponse Entities.MsgHandler.MsgHandlerSoap.XmlString2FlatFile(Entities.MsgHandler.XmlString2FlatFileRequest request) {
            return base.Channel.XmlString2FlatFile(request);
        }
        
        public int XmlString2FlatFile(string xmlRequest, out string tita) {
            Entities.MsgHandler.XmlString2FlatFileRequest inValue = new Entities.MsgHandler.XmlString2FlatFileRequest();
            inValue.xmlRequest = xmlRequest;
            Entities.MsgHandler.XmlString2FlatFileResponse retVal = ((Entities.MsgHandler.MsgHandlerSoap)(this)).XmlString2FlatFile(inValue);
            tita = retVal.tita;
            return retVal.XmlString2FlatFileResult;
        }
        
        public System.Threading.Tasks.Task<Entities.MsgHandler.XmlString2FlatFileResponse> XmlString2FlatFileAsync(Entities.MsgHandler.XmlString2FlatFileRequest request) {
            return base.Channel.XmlString2FlatFileAsync(request);
        }
    }
}
