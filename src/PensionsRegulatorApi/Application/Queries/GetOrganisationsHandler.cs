﻿using System;
using System.Collections.Generic;
using MediatR;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    

    public class GetOrganisationsHandler : RequestHandler<GetOrganisations, IEnumerable<Organisation>>
    {
        private readonly IOrganisationRepository _repository;

        public GetOrganisationsHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        protected override IEnumerable<Organisation> Handle(GetOrganisations request)
        {
            return
                _repository
                    .GetOrganisationsForPAYEReference(
                        request.PAYEReference);
        }
    }
}