# MMLib.MediatR.Generators

## Očakávania

- [ ] Vygeneruje controller
- [ ] `POST` / `GET` / `PUT` / `PATCH` / `DELETE` metódy
  - [ ] Ako vyriešiť dotazy, ktoré majú skalárne parametre? (`DeleteCompany(long companyId)`, `UpdateCompany(long companyId, UpdateCompanyCommand command)`, `Task<IEnumerable<GetAllCompaniesQuery.Company>> Get()`)
  - [ ] Možno použiť parsovanie template? `"{id:long}"`
  - [ ] Možno aj nejaká factory `UpdateReservationCommand.DateChanged(id, command.DateTime)`
  - [ ] Atribúty `[FromRoute]`, `[FromQuery]`, `[FillPropertiesFromRoute]`
- [ ] Bude možné zadať názov controllera, pokiaľ nebude zadaný tak sa odvodí od názvy triedy
- [ ] Názov metódy generovať na základe tpy metódy a typu requestu. Ak je vyplený tak ho prebrať. Ak je to nested class tak zobrať nested class.
- [ ] Atribúty nad handle metódou sa prevezmú nad metódu triedy / ak je to nested tak to bude názov vynútený
- [ ] Prebrať atribúty parametra ako `[FromQuery]` / `[FromBody]`
- [ ] Prebrať komentáre z metódy
- [ ] Umožniť prepísať šablónu controllera
  - [ ] Globálne pridávanie atribútov
  - [ ] Dedenie z iného controllera
  - [ ] ...
- [ ] Umožniť prepísať šablónu metódu
    - [ ] Nejaké časti ako before, execute, after. Nech si tam môže človek spraviť čo potrebuje
- [ ] Umožniť definovať šablónu pre konkrétny controller
- [ ] Controller spraviť ako partial class
  - [ ] Umožniť dediť controller z niečoho špecifického
  - [ ] Umožniť definovať atribúty controllera
  - [ ] Prevziať komentáre controllera
- [ ] Verziovanie?
- [ ] Attribut, ktorým by som mohol dať odkaz statickú metódu, ktorá by niečo spracovala.