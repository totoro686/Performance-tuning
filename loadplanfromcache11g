--
-- load qep information into the plan_table from gv$sql_plan
--
-- requires you to identify the inst_id,sql_id,child_number first
--
-- parameter 1 = inst_id
-- parameter 2 = sql_id
-- parameter 3 = child_number
--
-- @loadplanfromcache11g.sql  1  6zcb0r0rch025  0
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
from gv$sql_plan
where inst_id = &&1
and sql_id = '&&2'
and child_number = &&3
/
