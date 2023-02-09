// See https://aka.ms/new-console-template for more information
using s2industries.ZUGFeRD;
using TXTextControl.DocumentServer.PDF.Zugferd;

Console.WriteLine("Creating ZUGFeRD Invoice...");

Order order = new Order() {
	Buyer = new ContractParty() {
		Address = "123 Street Dr.",
		CountryCodes = CountryCodes.US,
		BuyerContact = new ContractPartyContact() {
			FirstName = "Jack",
			LastName = "Sparrow"
		},
		City = "Charlotte",
		Id = 123,
		Name = "Company, LLC",
		PostalCode = "NC 28209"
	},
	Seller = new ContractParty() {
		Address = "333 Ave Dr.",
		CountryCodes = CountryCodes.US,
		BuyerContact = new ContractPartyContact() {
			FirstName = "Peter",
			LastName = "Jackson"
		},
		City = "Charlotte",
		Id = 123,
		Name = "Microogle, LLC",
		PostalCode = "NC 28210"
	},
	Id = 1,
	OrderDate = DateTime.Now,
	Allowance = 0,
	Vat = 6.5M,
	LineItems = new List<LineItem> {
					new LineItem() {
						Name = "Product A",
						Description = "Description A",
						Quantity = 5,
						QuantityCode = QuantityCodes.H87,
						GrossPrice = 432.00M
					},
					new LineItem() {
						Name = "Product B",
						Description = "Description B",
						Quantity = 2,
						QuantityCode = QuantityCodes.H87,
						GrossPrice = 5232.00M
					},
				}
};

File.WriteAllBytes("result.pdf", Invoice.Create(order));

Console.WriteLine("ZUGFeRD Invoice created successfully!");