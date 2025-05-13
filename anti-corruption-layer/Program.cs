ILegacyCustomerService legacyService = new LegacyCustomerService();
var adapter = new CustomerAdapter(legacyService);

var newCustomer = new Customer
{
    Id = Guid.NewGuid(),
    FullName = "Alice Johnson",
    Email = "alice@example.com"
};

adapter.SaveCustomer(newCustomer);
Console.WriteLine("Customer saved via adapter.\n");

var retrievedCustomer = adapter.GetCustomer(newCustomer.Id);
Console.WriteLine($"Retrieved Customer:\nName: {retrievedCustomer.FullName}\nEmail: {retrievedCustomer.Email}");


    // Domain Model
    public class Customer
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    // Legacy DTO
    public class LegacyCustomerDto
    {
        public int CustomerCode { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }
    }

    // Translator
    public static class CustomerTranslator
    {
        public static Customer ToDomainModel(LegacyCustomerDto legacyCustomer)
        {
            return new Customer
            {
                Id = Guid.NewGuid(),
                FullName = legacyCustomer.Name,
                Email = legacyCustomer.ContactEmail
            };
        }

        public static LegacyCustomerDto ToLegacyDto(Customer customer)
        {
            return new LegacyCustomerDto
            {
                CustomerCode = customer.Id.GetHashCode(),
                Name = customer.FullName,
                ContactEmail = customer.Email
            };
        }
    }

    // Legacy Interface
    public interface ILegacyCustomerService
    {
        LegacyCustomerDto GetCustomerByCode(int code);
        void SaveCustomer(LegacyCustomerDto customer);
    }

    // Fake Legacy Service Implementation
    public class LegacyCustomerService : ILegacyCustomerService
    {
        private readonly Dictionary<int, LegacyCustomerDto> _storage = new();

        public LegacyCustomerDto GetCustomerByCode(int code)
        {
            return _storage.ContainsKey(code) ? _storage[code] : null;
        }

        public void SaveCustomer(LegacyCustomerDto customer)
        {
            _storage[customer.CustomerCode] = customer;
        }
    }

    // Anti-Corruption Adapter
    public class CustomerAdapter
    {
        private readonly ILegacyCustomerService _legacyService;

        public CustomerAdapter(ILegacyCustomerService legacyService)
        {
            _legacyService = legacyService;
        }

        public Customer GetCustomer(Guid domainId)
        {
            int legacyCode = domainId.GetHashCode();
            var legacyDto = _legacyService.GetCustomerByCode(legacyCode);
            return legacyDto != null ? CustomerTranslator.ToDomainModel(legacyDto) : null;
        }

        public void SaveCustomer(Customer customer)
        {
            var legacyDto = CustomerTranslator.ToLegacyDto(customer);
            _legacyService.SaveCustomer(legacyDto);
        }
    }