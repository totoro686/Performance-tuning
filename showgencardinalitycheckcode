/*

@gencardinalitycheckcode.sql dwstage ods_pega_report_group_assoc CASEID,CVRGPLNNBR,PLCYID,PEGARPRTGRPSTRTDT,LSSUNTNBR,FNDNGMTHDPLNNBR,EMPGRPID,CVRGCTGRYCD,CVRGTYPCD,FNDNGMTHDCD,RPRTGRPID,ROW_TERM_DATE,SOURCE 1
@gencardinalitycheckcode.sql dwstage ods_pega_report_group_assoc CASEID,CVRGPLNNBR,PLCYID,PEGARPRTGRPSTRTDT,LSSUNTNBR,FNDNGMTHDPLNNBR,EMPGRPID,CVRGCTGRYCD,CVRGTYPCD,FNDNGMTHDCD,RPRTGRPID,ROW_TERM_DATE,SOURCE 2
@gencardinalitycheckcode.sql dwstage ods_pega_report_group_assoc CASEID,CVRGPLNNBR,PLCYID,PEGARPRTGRPSTRTDT,LSSUNTNBR,FNDNGMTHDPLNNBR,EMPGRPID,CVRGCTGRYCD,CVRGTYPCD,FNDNGMTHDCD,RPRTGRPID,ROW_TERM_DATE,SOURCE 3
@gencardinalitycheckcode.sql dwstage ods_pega_report_group_assoc CASEID,CVRGPLNNBR,PLCYID,PEGARPRTGRPSTRTDT,LSSUNTNBR,FNDNGMTHDPLNNBR,EMPGRPID,CVRGCTGRYCD,CVRGTYPCD,FNDNGMTHDCD,RPRTGRPID,ROW_TERM_DATE,SOURCE 4


use this to get a starting point for best combination of columns based on distinct
note this still have the NULLS issue maybe (how do we handle columns with large % of null values)
but ignoring this, the technique is pretty solid

generate code and run it to find out how distinct different combinations of columns are
give some list of columns generate the possible combinations of columns restricted to a limited number (exp. combinations of N things taken M at a time)
results should point you to a good starting set of columns for an index given you list of columns you are intersted in
from this you use GENCARDINALITYCHECKCODE2.SQL

*/

with
       table_data as (
                       select owner,table_name
                       from dba_tables
                       where owner = upper('&&1')
                       and table_name = upper('&&2')
                      )
     , column_list as (
                        select dba_tab_columns.owner,dba_tab_columns.table_name,dba_tab_columns.column_name
                        from dba_tab_columns
                            ,table_data
                        where dba_tab_columns.owner = table_data.owner
                        and dba_tab_columns.table_name = table_data.table_name
                        and instr(','||upper('&&3')||',',','||dba_tab_columns.column_name||',') > 0
                      )
     , column_expression as (
                              select a.*
                                    ,length(sys_connect_by_path(a.column_name,','))-length(replace(sys_connect_by_path(a.column_name,','),',')) column_count
                                    ,substr(sys_connect_by_path(a.column_name,'||'',''||'),8) column_expression
                              from column_list a
                              connect by prior a.column_name < a.column_name
                            )
select 'clear columns' from dual union all
select 'col column_count newline' from dual union all
select 'col COLUMN_EXPRESSION format a800' from dual union all
select 'set linesize 999' from dual union all
select 'set pagesize 0' from dual union all
select 'set trimspool on' from dual union all
select 'set trimout on' from dual union all
--select 'set feedback off' from dual union all
--select 'set timing off' from dual union all
--select 'set time off' from dual union all
select '--owner table_name rowcount number_of_columns column_combo_cardinality column_expression' from dual union all
select 'select '''||table_data.owner||''' owner,'''||table_data.table_name||''' table_name,count(*) table_rowcount'
from table_data
union all
select *
from (
       select '      ,'||column_expression.column_count||' column_count,count(distinct '||column_expression.column_expression||') expression_rowcount,'''||replace(column_expression.column_expression,chr(39),chr(39)||chr(39))||''' column_expression'
       from column_expression
       where column_count <= &&4
       order by column_count,column_expression
     )
union all
select 'from '||table_data.owner||'.'||table_data.table_name
from table_data
union all
select '/'
from table_data
/
