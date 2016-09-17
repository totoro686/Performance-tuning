--
-- show filter queries for the most recent plan in the plan_table
--
-- usage is: @SHOWPLANFILTERQUERIES11G
--

with
     table_list as (
                     select 'TABLE' plan_object_type,a.id,a.object_owner table_owner,a.object_name table_name,a.access_predicates,a.filter_predicates,a.object_alias,a.cardinality
                     from plan_table a
                         ,dba_tables b
                     where b.owner = a.object_owner
                     and b.table_name = a.object_name
                     and a.plan_id = (select max(plan_id) from plan_table)
                     union all
                     select 'INDEX' plan_object_type,a.id,b.table_owner,b.table_name object_name,a.access_predicates,a.filter_predicates,a.object_alias,a.cardinality
                     from plan_table a
                         ,dba_indexes b
                     where b.owner = a.object_owner
                     and b.index_name = a.object_name
                     and a.plan_id = (select max(plan_id) from plan_table)
                   )
--
-- given the raw data for tables, modify the predicates so that we only see predicates for constant tests, no join predicates
-- join predicates are not used in FRP analysis
-- this is a bit of a hack as I never took the COMPILER and PARSER classes in school, basically this means it is almost 100%right
--
    , modified_table_list as (
                               select id,table_owner,table_name,object_alias,cardinality,plan_object_type
                             ,case when
                                        instr(replace(access_predicates,'"="'),'=') > 0 or
                                        instr(replace(access_predicates,'">"'),'>') > 0 or
                                        instr(replace(access_predicates,'"<"'),'<') > 0 or
                                        instr(replace(access_predicates,'">="'),'>=') > 0 or
                                        instr(replace(access_predicates,'"<="'),'<=') > 0 or
                                        instr(replace(access_predicates,'"!="'),'!=') > 0 or
                                        instr(replace(access_predicates,'"<>"'),'<>') > 0 or
                                        instr(replace(access_predicates,'" LIKE "'),' LIKE ') > 0 or
                                        instr(replace(access_predicates,'" BETWEEN "'),' BETWEEN ') > 0 or
                                        instr(replace(access_predicates,'" IN ("'),' IN (') > 0 or
                                        instr(replace(access_predicates,'" NOT LIKE "'),' NOT LIKE ') > 0 or
                                        instr(replace(access_predicates,'" NOT BETWEEN "'),' NOT BETWEEN ') > 0 or
                                        instr(replace(access_predicates,'" NOT IN ("'),' NOT IN (') > 0
                                   then access_predicates
                              end access_predicates
                             ,case when
                                        instr(replace(filter_predicates,'"="'),'=') > 0 or
                                        instr(replace(filter_predicates,'">"'),'>') > 0 or
                                        instr(replace(filter_predicates,'"<"'),'<') > 0 or
                                        instr(replace(filter_predicates,'">="'),'>=') > 0 or
                                        instr(replace(filter_predicates,'"<="'),'<=') > 0 or
                                        instr(replace(filter_predicates,'"!="'),'!=') > 0 or
                                        instr(replace(filter_predicates,'"<>"'),'<>') > 0 or
                                        instr(replace(filter_predicates,'" LIKE "'),' LIKE ') > 0 or
                                        instr(replace(filter_predicates,'" BETWEEN "'),' BETWEEN ') > 0 or
                                        instr(replace(filter_predicates,'" IN ("'),' IN (') > 0 or
                                        instr(replace(filter_predicates,'" NOT LIKE "'),' NOT LIKE ') > 0 or
                                        instr(replace(filter_predicates,'" NOT BETWEEN "'),' NOT BETWEEN ') > 0 or
                                        instr(replace(filter_predicates,'" NOT IN ("'),' NOT IN (') > 0
                                   then filter_predicates
                              end filter_predicates
                            from table_list
                             )
--
-- do the final massaging of the raw data
-- in particular, get the true alias for each table, get data from dba_tables, generate an actual predicate we can test with
--
    , plan_info as
                   (
                     select
                              id
                            , table_owner
                            , table_name
                            , substr(object_alias,1,instr(object_alias,'@')-1) table_alias
                            , cardinality
                            , (select num_rows from dba_tables where dba_tables.owner = modified_table_list.table_owner and dba_tables.table_name = modified_table_list.table_name) num_rows
                            , case
                                   when access_predicates is null and filter_predicates is null then null
                                   when access_predicates is null and filter_predicates is not null then filter_predicates
                                   when access_predicates is not null and filter_predicates is null then access_predicates
                                   when access_predicates is not null and filter_predicates is not null and access_predicates != filter_predicates then access_predicates||' and '||filter_predicates
                                   else access_predicates
                              end predicate
                     from modified_table_list
                   )
--
-- look for places where indexes are accessed followed by table acces by rowid
-- combine the two lines into one
--
    , combined_plan_info as (
                              select plan_info.table_owner,plan_info.table_name,plan_info.table_alias,plan_info.num_rows
                                    ,min(plan_info.id) id
                                    ,max(plan_info.cardinality) cardinality
                                    ,listagg(plan_info.predicate,' and ') within group (order by id) predicate
                              from plan_info
                              group by plan_info.table_owner,plan_info.table_name,plan_info.table_alias,plan_info.num_rows
                            )
select 'select count(*) filtered_rowcount,'''||table_owner||''' table_owner,'''||table_name||''' table_name,'''||table_alias||''' table_alias from '||table_owner||'.'||table_name||' '||table_alias||decode(predicate,null,null,'  where '||predicate)
      ||decode(lead(table_name) over (order by table_owner,table_name),null,' ;',' union all')
from combined_plan_info 
order by table_owner,table_name
/

