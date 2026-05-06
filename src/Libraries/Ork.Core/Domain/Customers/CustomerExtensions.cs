namespace Ork.Core.Domain.Customers;

public static class CustomerExtensions
{
    public static bool IsSearchEngineAccount(this Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));
        if (!customer.IsSystemAccount || string.IsNullOrWhiteSpace(customer.SystemName))
        {
            return false;
        }

        bool result = customer.SystemName.Equals(
            OrkCustomerDefaults.SearchEngineCustomerName, 
            StringComparison.InvariantCultureIgnoreCase);

        return result;
    }

    public static bool IsBackgroundTaskAccount(this Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));
        if (!customer.IsSystemAccount || string.IsNullOrWhiteSpace(customer.SystemName))
        {
            return false;

        }
        bool result = customer.SystemName.Equals(
            OrkCustomerDefaults.BackgroundTaskCustomerName, 
            StringComparison.InvariantCultureIgnoreCase);

        return result;
    }
}
