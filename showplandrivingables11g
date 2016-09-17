--
-- show the driving table for the most recent query plan in the plan_table
--
-- usage is: @SHOWPLANDRIVINGTABLES11G
--

set linesize 999
col driving_table format a61
col driving_table_alias format a30
col leading_hint format a300


select id
      ,object_owner||'.'||object_name driving_table
      ,driving_table_alias
      ,object_type
      ,leading_hint
from (
       select substr(replace(leading_hint,')',' ')
                    ,instr(replace(leading_hint,')',' '),' ',1,1)+1
                    ,instr(replace(leading_hint,')',' '),' ',1,2)-instr(replace(leading_hint,')',' '),' ',1,1)-1) driving_table_alias
             ,leading_hint
       from (
              select substr(c1,1,instr(c1,')')) leading_hint
              from (
                     select substr(other_xml,instr(other_xml,'LEADING(')) c1
                     from plan_table
                     where other_xml is not null
                     and plan_id = (select max(plan_id) from plan_table)
                   )
            )
     ) x
    ,plan_table
where plan_table.object_alias = trim(replace(to_char(substr(x.driving_table_alias,1,4000)),'"'))
and plan_table.plan_id = (select max(plan_id) from plan_table)
/
