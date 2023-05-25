namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;

public enum ValidationResultType
{
    Success,
    PartialSuccess,
    Failure,
    NotEnoughData,
    Junk,
    HasAlternatives,
    Throwaway,
    Corrected,
    Several,
    Foreign,
    BrandRecognized
}
