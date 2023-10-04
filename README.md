# Digital Apprenticeships Service

## Pensions Regulator API

Licensed under the [MIT license](https://github.com/SkillsFundingAgency/das-assessor-service/blob/master/LICENSE.txt)

|               |               |
| ------------- | ------------- |
|![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png)|Pensions Regulator API |
| Info | An API which searches Pensions Regulator data for matching PAYE and AORN |
| Build | [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-pensionsregulator?repoName=SkillsFundingAgency%2Fdas-pensionsregulator&branchName=CON-4010_Use_MI_authentication)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1588&repoName=SkillsFundingAgency%2Fdas-pensionsregulator&branchName=master) |


### Developer Setup

#### Requirements

- Install [.NET Core 3.1 SDK](https://www.microsoft.com/net/download)
- Install [Visual Studio 2019](https://www.visualstudio.com/downloads/) with these workloads:
    - ASP.NET and web development
- Install [SQL Server 2017 Developer Edition](https://go.microsoft.com/fwlink/?linkid=853016)
- Install [SQL Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Install [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) (Make sure you are on atleast v5.3)

- Administrator Access

#### Setup

- Clone this repository
- Open Visual Studio as an administrator

##### Publish Database

- Build the solution das-pensions-regulator.sln
- Either use Visual Studio's `Publish Database` tool to publish the database project SFA.DAS.PensionsRegulator.Database to name {{database name}} on {{local instance name}}

	or

- Create a database manually named {{database name}} on {{local instance name}} and run each of the `.sql` scripts in the SFA.DAS.PensionsRegulator.Database project.

##### Config

- Get the das-pensions-regulator configuration json file from [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-pensions-regulator/SFA.DAS.PensionsRegulatorApi.json); which is a non-public repository.
- Create a Configuration table in your (Development) local Azure Storage account.
- Add a row to the Configuration table with fields: PartitionKey: LOCAL, RowKey: SFA.DAS.PensionsRegulatorApi_1.0, Data: {{The contents of the local config json file}}.
- Update Configuration SFA.DAS.PensionsRegulatorApi_1.0, Data { "SqlConnectionstring":"Server={{local instance name}};Initial Catalog={{database name}};Trusted_Connection=True;" }

### Data

The source of the data which is searched is the [das-data-factory](https://github.com/SkillsFundingAgency/das-data-factory) repository.

### Deployment requirements

#### Permissions

- The Entra ID Service Principal used by ADO to deploy this service must have at least Contributor access at the subscription scope and either:
    - the User Access Administrator at the subscription scope for the entire ARM template to deploy successfully first time
    - or the User Access Administrator at the Data Share and Storage Acccount resource scopes to ensure the role assignment deployments are successful
