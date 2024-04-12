create table TBLUSER
(
    USERNO  integer not null
        constraint TBLUSER_pk
            primary key,
    USERNM  VARCHAR not null,
    USERSEX VARCHAR not null
);

