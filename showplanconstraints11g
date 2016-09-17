--
-- show all PK/UK/FK constraints for any table referenced by most recent query plan in the plan_table
-- note that table references may be indirect and that not all tables in a query need be used by a query plan
--
-- usage is: @SHOWPLANCONSTRAINTS11G
--

break on constraint_name skip 1 on parent_table_name
col column_name format a30
col sort_order noprint
col select_id noprint

with
      plan_references as (
                        select id,object_owner owner,object_name table_name,replace(object_alias,'@','   @   ') object_alias
                        from plan_table
                        where object_type = 'TABLE'
                        and plan_id = (select max(plan_id) from plan_table)
                        union
                        select id,b.table_owner,b.table_name,replace(object_alias,'@','   @   ') object_alias
                        from plan_table a
                            ,dba_indexes b
                        where a.object_type like 'INDEX%'
                        and a.object_owner = b.owner
                        and a.object_name = b.index_name
                        and a.plan_id = (select max(plan_id) from plan_table)
                     )
   , plan_tables as (
                      select distinct owner,table_name
                      from plan_references
                    )
select *
from (
select a.owner,a.table_name,a.constraint_name,a.constraint_type,c.column_name,b.owner parent_child_owner,b.table_name parent_child_table_name,b.constraint_type parent_constraint_type,a.index_name,decode(a.constraint_type,'P',1,'U',2) sort_order,1 select_id
from dba_constraints a
    ,dba_constraints b
    ,dba_cons_columns c
where (a.owner,a.table_name) in (select owner,table_name from plan_tables)
and a.r_owner = b.owner(+)
and a.r_constraint_name = b.constraint_name(+)
and a.owner = c.owner
and a.constraint_name = c.constraint_name
and a.constraint_type != 'C'
and (b.owner is null and a.constraint_type in ('P','U') or (b.owner,b.table_name) in (select owner,table_name from plan_tables))
union
select a.owner,a.table_name,a.constraint_name,a.constraint_type,c.column_name,b.owner parent_child_owner,b.table_name parent_child_table_name,b.constraint_type parent_constraint_type,a.index_name,decode(a.constraint_type,'P',1,'U',2) sort_order,2 select_id
from dba_constraints a
    ,dba_constraints b
    ,dba_cons_columns c
where (a.owner,a.table_name) in (select owner,table_name from plan_tables)
and b.constraint_type in ('P','U')
and a.r_owner = b.owner
and a.r_constraint_name = b.constraint_name
and b.table_name != a.table_name
and a.owner = c.owner
and a.constraint_name = c.constraint_name
and (b.owner,b.table_name) in (select owner,table_name from plan_tables)
)
order by select_id,sort_order,owner,table_name,constraint_name,column_name
/
