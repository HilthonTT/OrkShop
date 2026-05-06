using System.Diagnostics.CodeAnalysis;

namespace Ork.Core.Domain.Customers;

public sealed class CustomerRoleComparerByName : IEqualityComparer<CustomerRole>
{
    public bool Equals(CustomerRole? x, CustomerRole? y)
    {
        // Check if both objects are the same reference
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return x.Name == y.Name && x.SystemName == y.SystemName;
    }

    public int GetHashCode([DisallowNull] CustomerRole customer)
    {
        if (customer is null)
        {
            return 0;
        }

        // Get hash code for the Name field if it is not null.
        int hashCustomerRoleName = customer.Name is null ? 0 : customer.Name.GetHashCode();

        // Get hash code for the SystemName field.
        int hashCustomerRoleSystemName = customer.SystemName is null ? 0 : customer.SystemName.GetHashCode();

        // Calculate the hash code for the CustomerRole.
        return hashCustomerRoleName ^ hashCustomerRoleSystemName;
    }
}
