--
-- create a multi-part report on database SQL by instance
-- grouping SQL into powers of ten based on cpu consumed
-- in order to see easily the top 1% of queries that are most busy
--

set linesize 999
set pagesize 50000
set feedback 1
set trimspool on
set trimout on
set doc off
clear breaks
clear computes
set echo off

-- alter session set nls_date_format = 'dd-mon-rrrr hh24:mi:ss';

select * from v$version
/

select inst_id
      ,instance_name
      ,startup_time
      ,round((sysdate-startup_time),1) up_days
      ,round(round((sysdate-startup_time),1)*24) maximum_cache_hours
      ,to_char(sysdate,'dd-mon-rrrr hh24:mi:ss') right_now
from gv$instance
order by inst_id
/

--
-- show how long the database has been up, how many cpus are available, and thus the theoretical CPU available
--
select inst_id
      ,instance_name
      ,(select cpu_count_highwater from sys.v_$license) cpu_count
      ,round((round((sysdate-startup_time),1)*24)*(select cpu_count_highwater from sys.v_$license)) available_cpu_hours
from gv$instance
order by inst_id
/

col sql_text format a700 trunc
col sql_text clear
col sql_text format a700 trunc
col pct_total format 990
compute sum of sql_statements on inst_id
compute sum of sql_statements on report
compute sum of db_pct_total on inst_id
compute sum of db_pct_total on report
compute sum of running_consumed_cpu_hours on report
break on inst_id skip page on report

--
-- use a logarithmic scale to plot cpu consumtion of all queries in the cache
-- we can use this to zero in on the top consumers
-- notice we exclude PLSQL CALLS but not the sql inside these calls
-- this gives us the true SQL workload
--
select 
       inst_id
      ,cpu_time_log10
      ,sql_statements
      ,cpu_time_rounded,round(cpu_time) cpu_time
      ,round(100*ratio_to_report(cpu_time) over(partition by inst_id)) inst_pct_total
      ,round(100*ratio_to_report(cpu_time) over()) db_pct_total
      ,round(sum(cpu_time) over (partition by inst_id order by cpu_time_log10)) running_cpu_time
      ,round(round(sum(cpu_time) over (partition by inst_id order by cpu_time_log10))/60/60,2) running_consumed_cpu_hours
from (
        select 
               inst_id
              ,trunc(log(10,mycpu_time)) cpu_time_log10
              ,count(*) sql_statements
              ,power(10,trunc(log(10,mycpu_time))) cpu_time_rounded
              ,sum(mycpu_time) cpu_time
        from (
               select inst_id,case when cpu_time <= 0 then 1 else cpu_time end/1000000 mycpu_time
               from gv$sqlarea
               where trim(upper(sql_text)) not like 'BEGIN%'
               and trim(upper(sql_text)) not like 'DECLARE%'
               and trim(upper(sql_text)) not like 'CALL%'
             )
        group by trunc(log(10,mycpu_time)),inst_id
     ) a
order by inst_id,a.cpu_time_log10
/

clear computes
col module format a30 word
col sec_per_exec format 999999990.0
--compute sum of cpu_seconds on inst_id , report
break on inst_id skip page
--
-- show use the actual SQL that exceeds some threshhold
-- these are the queries we want to concentrate on
-- configure the last AND predicate to whatever is reasonable based on the above query
--
select inst_id
      ,sql_id
      ,child_number
      ,trunc(cpu_time/1000000) cpu_seconds
      ,trunc(elapsed_time/1000000) eplapsed_seconds
      ,executions
      ,round(trunc(elapsed_time/1000000)/decode(executions,0,null,executions),1) sec_per_exec
      ,round((sysdate-to_date(first_load_time,'rrrr-mm-dd/hh24:mi:ss'))*24) hours_in_cache
      ,module
--      ,address
      ,hash_value
      ,(select 'Open' from gv$open_cursor b where b.inst_id = a.inst_id and b.address = a.address and b.hash_value = a.hash_value and rownum = 1) open
/*
      ,case when sql_text like '%SELECT /*+ ORDERED NO_EXPAND USE_HASH%' or sql_text like '%FROM :Q%' or instr(sql_text,'CIV_GB') > 0 or instr(sql_text,'PIV_GB') > 0 or instr(sql_text,'PIV_SSF') > 0 or instr(sql_text,'SWAP_JOIN_INPUTS') > 0 then 'Slave'
            when sql_text like '%SELECT /*+ Q%' then 'Query'
            else (select 'Yes' from gv$sql_plan b where b.inst_id = a.inst_id and b.address = a.address and b.hash_value = a.hash_value and b.child_number = a.child_number and b.other_tag like 'PARALLEL%' and rownum = 1) 
       end parallel
*/
      ,sql_text
from gv$sql a
where trim(upper(sql_text)) not like 'BEGIN%'
and trim(upper(sql_text)) not like 'DECLARE%'
and trim(upper(sql_text)) not like 'CALL%'
and cpu_time/1000000 >= 1000
--and cpu_time/1000000 >= 100
order by 1,4,5
/
