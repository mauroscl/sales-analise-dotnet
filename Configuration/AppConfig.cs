using System;

namespace Configuration
{
    public static class AppConfig
    {
        public const string InputPathEnv = "INPUT_PATH";
        public const string OuputPathEnv = "OUTPUT_PATH";
        public const string KafkaServerEnv = "KAFKA_SERVER";
        public const string ApplicationIdEnv = "APPLICATION_ID";

        public const string KafkaFileNameHeader = "CTM_FILE_NAME";
        public const string KafkaSalesAnalysisOutputTopicHeader = "CTM_SALES_ANALYSIS_OUTPUT_TOPIC";

        public static string GetSalesAnalysisOutputTopic()
        {
            return "sales-analysis-output-" + Environment.GetEnvironmentVariable(ApplicationIdEnv);
        }
    }
}
