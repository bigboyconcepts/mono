//
// System.Web.Compilation.WsdlBuildProvider
//
// Authors:
//	Marek Habersack <grendello@gmail.com>
//
// (C) 2006 Marek Habersack
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if NET_2_0 && WEBSERVICES_DEP

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace System.Web.Compilation {

	[BuildProviderAppliesTo (BuildProviderAppliesTo.Web|BuildProviderAppliesTo.Code)]
	sealed class WsdlBuildProvider : BuildProvider {
		public WsdlBuildProvider()
		{
		}

		public override void GenerateCode (AssemblyBuilder assemblyBuilder)
		{
			CodeCompileUnit unit = new CodeCompileUnit ();
			CodeNamespace proxyCode = new CodeNamespace(null);
			unit.Namespaces.Add (proxyCode);
			
			string path = HttpContext.Current.Request.MapPath (VirtualPath);
			ServiceDescription description = ServiceDescription.Read (path);
			ServiceDescriptionImporter importer = new ServiceDescriptionImporter ();
				
			importer.AddServiceDescription (description, null, null);
			importer.Style = ServiceDescriptionImportStyle.Client;
			importer.CodeGenerator = assemblyBuilder.CodeDomProvider;
			importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
			importer.Import (proxyCode, unit);
			assemblyBuilder.AddCodeCompileUnit (unit);
		}
	}
}
#endif

