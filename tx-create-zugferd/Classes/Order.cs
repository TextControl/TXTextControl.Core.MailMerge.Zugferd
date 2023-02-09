using s2industries.ZUGFeRD;

namespace TXTextControl.DocumentServer.PDF.Zugferd {
	public class Order {
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public ContractParty Buyer { get; set; }
		public ContractParty Seller { get; set; }
		public List<LineItem> LineItems { get; set; } = new List<LineItem>();
		public Decimal LineTotalAmount {
			get {
				return this.LineItems.Sum(item => item.GrossPrice * item.Quantity);
			}
		}

		public Decimal TaxTotalAmount {
			get {
				return (this.LineTotalAmount / 100) * this.Vat;
			}
		}

		public Decimal GrandTotalAmount {
			get {
				return this.LineTotalAmount + this.TaxTotalAmount;
			}
		}

		public Decimal Allowance { get; set; }
		public Decimal Vat { get; set; } = 6.5M;

		public byte[] ZugferdXML {
			get {
				var invoice = InvoiceDescriptor.CreateInvoice(this.Id.ToString(),
						this.OrderDate, CurrencyCodes.USD);

				invoice.SetBuyer(this.Buyer.Name,
					this.Buyer.PostalCode,
					this.Buyer.City,
					this.Buyer.Address,
					this.Buyer.CountryCodes,
					this.Buyer.Id.ToString());

				invoice.SetSeller(this.Seller.Name,
					this.Seller.PostalCode,
					this.Seller.City,
					this.Seller.Address,
					this.Seller.CountryCodes,
					this.Seller.Id.ToString());

				invoice.SetSellerContact($"{this.Seller.BuyerContact.FirstName} {this.Seller.BuyerContact.LastName}");

				foreach (LineItem lineItem in this.LineItems) {
					invoice.AddTradeLineItem(lineItem.Name, lineItem.Description, lineItem.QuantityCode, lineItem.Quantity, lineItem.GrossPrice, lineItem.GrossPrice, lineItem.Quantity);
				}

				invoice.LineTotalAmount = LineTotalAmount;
				invoice.ChargeTotalAmount = invoice.LineTotalAmount;
				invoice.AllowanceTotalAmount = Allowance;
				invoice.TaxBasisAmount = invoice.ChargeTotalAmount;
				invoice.TaxTotalAmount = TaxTotalAmount;
				invoice.GrandTotalAmount = GrandTotalAmount;

				MemoryStream ms = new MemoryStream();

				invoice.Save(ms, ZUGFeRDVersion.Version1, Profile.Basic);

				return ms.ToArray();
			}
		}

	}

	public class LineItem {
		public string Name { get; set; }
		public string Description { get; set; }
		public QuantityCodes QuantityCode { get; set; } = QuantityCodes.H87;
		public int Quantity { get; set; }
		public decimal GrossPrice { get; set; }
	}

	public class Seller : ContractParty { }
	public class Buyer : ContractParty { }

	public class ContractParty {
		public int Id { get; set; }
		public string Name { get; set; }
		public string PostalCode { get; set; }
		public string City { get; set; }
		public string Address { get; set; }
		public CountryCodes CountryCodes { get; set; }
		public ContractPartyContact BuyerContact { get; set; }
	}

	public class ContractPartyContact {
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}


}
