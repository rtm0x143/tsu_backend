connect system/root

create user C##MovieCatalog identified by mcdevpass;
grant resource, create session, connect to C##MovieCatalog;
alter user C##MovieCatalog quota unlimited on USERS;

create user C##TokenList identified by tldevpass;
grant resource, create session, connect to C##TokenList;
alter user C##TokenList quota unlimited on USERS;

commit;
