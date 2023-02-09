namespace TXTextControl.DocumentServer.PDF.Zugferd {
	public static class Invoice {
		public static byte[] Create(Order order) {

			TXTextControl.SaveSettings saveSettings = new TXTextControl.SaveSettings();

			var metaData = System.IO.File.ReadAllText("metadata.xml");

			// create a new embedded file
			var zugferdInvoice = new TXTextControl.EmbeddedFile(
				"ZUGFeRD-invoice.xml",
				order.ZugferdXML,
				metaData);

			zugferdInvoice.Description = "ZUGFeRD-invoice";
			zugferdInvoice.Relationship = "Alternative";
			zugferdInvoice.MIMEType = "application/xml";
			zugferdInvoice.LastModificationDate = DateTime.Now;

			// set the embedded files
			saveSettings.EmbeddedFiles = new TXTextControl.EmbeddedFile[] { zugferdInvoice };

			byte[] document;

			using (TXTextControl.ServerTextControl tx = new ServerTextControl()) {
				tx.Create();

				tx.Load("template.tx", StreamType.InternalUnicodeFormat);

				using (MailMerge mailMerge = new MailMerge()) {
					mailMerge.TextComponent = tx;
					mailMerge.MergeObject(order);
				}

				// export the PDF
				tx.Save(out document, TXTextControl.BinaryStreamType.AdobePDFA, saveSettings);

				return document;
			}
		}
	}
	}

