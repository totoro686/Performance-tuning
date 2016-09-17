--
-- show indexes for any table referenced directly or indirectly in the current plan of the plan_table
--
-- usage is: @SHOWPLANINDEXES11G
--

break on table_owner on table_name on index_owner skip 1 on index_name skip 1 on uniqueness
col column_name format a30

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
select dba_ind_columns.column_name,dba_indexes.index_name,dba_indexes.uniqueness,dba_indexes.table_name,dba_indexes.table_owner,dba_indexes.owner index_owner
from dba_indexes
    ,dba_ind_columns
    ,plan_tables
where plan_tables.owner = dba_indexes.table_owner
and plan_tables.table_name = dba_indexes.table_name
and dba_indexes.owner = dba_ind_columns.index_owner
and dba_indexes.index_name = dba_ind_columns.index_name
order by dba_indexes.table_owner,dba_indexes.table_name,dba_indexes.owner,dba_indexes.index_name,dba_ind_columns.column_position
/
