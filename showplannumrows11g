--
-- get num_rows off DBA_TABLES for any table referenced by the current plan in the plan_table
--
-- usage is: @SHOWPLANNUMROWS11G
--

with
      table_list as (
                       select object_owner owner,object_name table_name
                       from plan_table
                       where object_type = 'TABLE'
                       and plan_id = (select max(plan_id) from plan_table)
                       union
                       select b.table_owner,b.table_name
                       from plan_table a
                           ,dba_indexes b
                       where a.object_type like 'INDEX%'
                       and a.object_owner = b.owner
                       and a.object_name = b.index_name
                       and a.plan_id = (select max(plan_id) from plan_table)
                    )
select num_rows,dba_tables.owner,dba_tables.table_name
from dba_tables
    ,table_list
where table_list.owner = dba_tables.owner
and table_list.table_name = dba_tables.table_name
order by 2,1
/
