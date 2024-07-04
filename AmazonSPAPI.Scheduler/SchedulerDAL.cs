using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonSPAPI.Scheduler
{
    public class SchedulerDAL
    {
        ILog log = LogManager.GetLogger("SchedulerDAL");
       
        string connectionString = ConfigurationManager.ConnectionStrings["SPAPIEntities"].ConnectionString;
        public void FeedIdUpdate(string FeedId, string Filename)
        {
            log.Info("SchedulerDAL Initiated");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            var filename = Path.GetFileName(Filename);
            if (filename.EndsWith(".txt"))
                filename = filename.Replace(".txt", "");

            log.Info("My Filename: "+$"{filename}");
            log.Info("My FeedId: "+FeedId);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = $"update amz.FeedSyncStatus set FeedId="+FeedId+",Status='Queued' where FeedFileName='"+filename+"'";

                    log.Info("MyQry: "+sqlQuery);

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Execute the UPDATE command
                        int rowsAffected = command.ExecuteNonQuery();
                        log.Info(rowsAffected);

                        connection.Close();

                        if (rowsAffected > 0)
                        {
                            return;
                        }
                        else
                        {
                            throw new Exception("row not Affected");
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                   // throw new Exception("Connection Issue");
                }
               
            }
        }
        public void FeedResultUpdate(string FeedId)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = $"update amz.FeedSyncStatus set Status='Completed',ModifiedOn=getdate() where FeedId='"+FeedId+"'";

                    log.Info(sqlQuery);

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Execute the UPDATE command
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();
                        log.Info(rowsAffected);

                        if (rowsAffected > 0)
                        {
                            return;

                        }
                        else
                        {
                            throw new Exception("row not Affected");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    //throw new Exception("Connection Issue");
                }
            }
        }
    }
}
