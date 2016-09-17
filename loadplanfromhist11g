--
-- load qep information into the plan_table from DBA_HIST_SQL_PLAN (history of monitored plans)
--
-- requires you to identify the sql_id and date of when you want your plan
-- does a bob barker lookup based on the date, for the sql_id supplied
-- note use of +1 second to the date in order to account for the millseconds in the timestamp
-- note also the use of a specific format mask 'rrrrmmddhh24miss'
-- often you will need to guess about the time unless you know it from some other place
--
-- parameter 1 = sql_id
-- parameter 2 = timestamp (in the right format)
--
-- @loadplanfromhist11g.sql  6zcb0r0rch025  2014080212:10:03
--
-- then use the normal plan_table scripts to get the goodness
--
-- @showplan11g
-- @showplandatamodel11g
-- ...
--
insert into plan_table
(
 PLAN_ID
,TIMESTAMP
,REMARKS
,OPERATION
,OPTIONS
,OBJECT_NODE
,OBJECT_OWNER
,OBJECT_NAME
,OBJECT_ALIAS
,object_instance
,OBJECT_TYPE
,OPTIMIZER
,SEARCH_COLUMNS
,ID
,PARENT_ID
,DEPTH
,POSITION
,COST
,CARDINALITY
,BYTES
,OTHER_TAG
,PARTITION_START
,PARTITION_STOP
,PARTITION_ID
,OTHER
,OTHER_XML
,DISTRIBUTION
,CPU_COST
,IO_COST
,TEMP_SPACE
,ACCESS_PREDICATES
,FILTER_PREDICATES
,PROJECTION
,TIME
,QBLOCK_NAME
)
select
  nvl((select max(plan_id) from plan_table),0)+1 plan_id
,TIMESTAMP
,REMARKS
,OPERATION
,OPTIONS
,OBJECT_NODE
,OBJECT_OWNER
,OBJECT_NAME
,OBJECT_ALIAS
,OBJECT#  object_instance
,OBJECT_TYPE
,OPTIMIZER
,SEARCH_COLUMNS
,ID
,PARENT_ID
,DEPTH
,POSITION
,COST
,CARDINALITY
,BYTES
,OTHER_TAG
,PARTITION_START
,PARTITION_STOP
,PARTITION_ID
,OTHER
,OTHER_XML
,DISTRIBUTION
,CPU_COST
,IO_COST
,TEMP_SPACE
,ACCESS_PREDICATES
,FILTER_PREDICATES
,PROJECTION
,TIME
,QBLOCK_NAME
from dba_hist_sql_plan
where sql_id = '&&1'
and timestamp = (
                 select max(timestamp)
                 from dba_hist_sql_plan
                 where sql_id = '&&1'
                 and timestamp <= to_date('&&2','rrrrmmddhh24miss')+1/24/60/60
                )
/
