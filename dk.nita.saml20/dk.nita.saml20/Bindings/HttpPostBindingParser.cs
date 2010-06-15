using System;
using System.Text;
using System.Web;
using System.Xml;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Bindings
{
    /// <summary>
    /// Parses the response messages related to the HTTP POST binding.
    /// </summary>
    public class HttpPostBindingParser
    {
        private readonly HttpContext _context;
        private XmlDocument _document;
        private bool _isResponse = false;
        private bool _isRequest = false;
        private string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostBindingParser"/> class.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        public HttpPostBindingParser(HttpContext context)
        {
            _context = context;

            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            string base64 = string.Empty;

            if (_context.Request.Params["SAMLRequest"] != null)
            {
                base64 = _context.Request.Params["SAMLRequest"];
                _isRequest = true;
            }
            if (_context.Request.Params["SAMLResponse"] != null)
            {
                base64 = _context.Request.Params["SAMLResponse"];
                _isResponse = true;
            }

            _message = Encoding.ASCII.GetString(Convert.FromBase64String(base64));

            _document = new XmlDocument();
            _document.PreserveWhitespace = true;
            _document.LoadXml(_message);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a response message.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a response message; otherwise, <c>false</c>.
        /// </value>
        public bool IsResponse
        {
            get{ return _isResponse;}
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a request message.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a request message; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequest
        {
            get { return _isRequest; }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>The document.</value>
        public XmlDocument Document
        {
            get { return _document; }
        }

        /// <summary>
        /// Checks the signature of the message.
        /// </summary>
        /// <returns></returns>
        public bool CheckSignature()
        {
            return XmlSignatureUtils.CheckSignature(_document);
        }

        /// <summary>
        /// Determines whether the message is signed.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the message is signed; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSigned()
        {
            return XmlSignatureUtils.IsSigned(_document);
        }
    
    }
}