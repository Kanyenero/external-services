namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;

public class AuthorizationData
{
    public string PublicToken { get; set; }

    public string SecretToken { get; set; }

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}", nameof(PublicToken), PublicToken, nameof(SecretToken), SecretToken);
}
