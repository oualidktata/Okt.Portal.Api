using Assette.Client;
using Framework.Common;
using Portal.Api.Repositories.Repos;
using System.Collections.Generic;

namespace Portal.Api.Repositories.Contracts
{

    public interface ICachebaleAssociationRepository : ICachebaleRepository<AssociationDto, AssociationDto, AssociationSimpleDto>
    {
        /// <summary>
        /// Remove Associations in bulk
        /// </summary>
        /// <returns></returns>
        IEnumerable<ResultObj<AssociationSimpleDto>> RemoveAssociations(AssociationDto[] associations);

        /// <summary>
        /// Remove Association
        /// </summary>
        /// <returns></returns>
        ResultObj<AssociationSimpleDto> RemoveAssociation(string userCode, string accountCode, string documenttypeCode);
        ResultObj<IEnumerable<AssociationSimpleDto>> FindBy(string userCode, string accountCode, string documentTypeCode);
    }
}