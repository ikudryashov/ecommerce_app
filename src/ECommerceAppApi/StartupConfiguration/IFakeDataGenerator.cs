namespace ECommerceAppApi.StartupConfiguration;

public interface IFakeDataGenerator
{
	public Task PopulateWithFakeDataAsync();
}