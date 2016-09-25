--
-- shows the histogram for a specific column
-- very usefull for showing when statistics might not help
--
-- parameter 1 = owner
-- parameter 2 = table_name
-- parameter 3 = column_name
--
-- usage is: @SHOWHISTOGRAM <owner> <table_name> <column_name>
--

COLUMN   endpoint_actual_value noprint
COLUMN   actual_value ON FORMAT   a30
col column format a30


select b.*
      ,round(ratio_to_report(cardinality) over(partition by owner,table_name,column_name)*100) pct
      ,endpoint_actual_value actual_value
from (
select a.*
      ,ENDPOINT_NUMBER-nvl(lag(ENDPOINT_NUMBER) over(partition by owner,table_name,column_name order by endpoint_number),0) cardinality
from dba_histograms a
where owner = upper('&&1')
and table_name = upper('&&2')
and column_name = upper('&&3')
) b
order by owner,table_name,column_name,ENDPOINT_NUMBER
/
