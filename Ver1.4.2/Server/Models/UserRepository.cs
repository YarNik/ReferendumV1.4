using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebServer.Models
{
    public interface IUserRepository
    {
        void Create(User user);
        void Delete(int id);
        User Get(int id);
        User Authorize(User user);
        //List<GetReferendums> GetReferendumsDb();
        List<SimpleReferendums> GetReferendumsDbNew();
        List<GetReferendums> GetSingleReferendumDb(User user, string id);
        List<MyReferendum> GetMyReferendumsDb(string id);
        List<MyReferendumNotPublic> GetMyReferendumsNotPublicDb(string id);
        List<ReceiveAllReferendums> ReceiveAllReferendumsDb();
        void Vote(AllAnswer answer);
        void AddReferendum(Referendum referendum);
        void SaveNewAnswer(AddAnswer answer);
        void Publish(string id);
        Referendum GetMyEditableReferendumDb(string id);
        List<AddAnswer> GetMyEditableReferendumAnswers(string id);
        void DeleteAnswerDb(string id);
        void ChangePropositionNameDb(ReceiveAllReferendums newName);
        void ChangeMaxOwnAnswersDb(Referendum edittedReferendum);
        void ChangeMaxAmountAnswersDb(Referendum edittedReferendum);
        void ChangeDeadLineDb(Referendum edittedReferendum);
    }
    public class UserRepository : IUserRepository
    {
        string connectionString = null;
        public UserRepository(string conn)
        {
            connectionString = conn;
        }

        /*public List<GetReferendums> GetReferendumsDb()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<GetReferendums>("SELECT Referendum.Id, Users.UserName AS Autor, Referendum.Proposition, Referendum.PublishDate FROM Referendum, Users WHERE Users.Id = Referendum.Autor").ToList();
            }
        }*/
        /*public List<GetReferendums> GetReferendumsDbNew(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<GetReferendums>("SELECT Users.UserName AS Autor, Referendum.Id, Referendum.Proposition, Answer.Appellation, Answer.id AS IdAnswer, COUNT(*) - 1  AS Amount, Referendum.MaxAmountAnswers - (SELECT COUNT(*) FROM AllAnswers WHERE AllAnswers.IdUser = @Id AND AllAnswers.idAnswer = Answer.id) AS VotesAmount, Referendum.DeadLine FROM Referendum, Answer, AllAnswers, Users WHERE Answer.Referendum = Referendum.id AND AllAnswers.idAnswer = Answer.id AND Users.Id = Referendum.Autor GROUP BY Answer.id, Answer.Appellation, Referendum.Proposition, Referendum.MaxAmountAnswers, Referendum.DeadLine, Referendum.Id, Users.UserName", new { Id = user.Id }).ToList();            
            }
        }*/

        public List<SimpleReferendums> GetReferendumsDbNew()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<SimpleReferendums>("SELECT Referendum.Id, Referendum.Published, Users.UserName AS Autor, Referendum.Proposition, Referendum.PublishDate FROM Referendum, Users WHERE Users.Id = Referendum.Autor").ToList();
            }
        }

        public List<GetReferendums> GetSingleReferendumDb(User user, string id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<GetReferendums>("SELECT Referendum.Id, Referendum.Proposition, Referendum.Published, Answer.Appellation, Answer.id AS IdAnswer, COUNT(*) - 1  AS Amount, Referendum.MaxAmountAnswers - (SELECT COUNT(*) FROM AllAnswers WHERE AllAnswers.IdUser = @Id AND AllAnswers.idAnswer = Answer.id) AS VotesAmount, Referendum.DeadLine FROM Referendum, Answer, AllAnswers WHERE Answer.Referendum = Referendum.id AND AllAnswers.idAnswer = Answer.id AND Referendum.Id = @idRefer GROUP BY Answer.id, Answer.Appellation, Referendum.Proposition, Referendum.MaxAmountAnswers, Referendum.DeadLine, Referendum.Id, Referendum.Published", new { Id = user.Id, idRefer = Int32.Parse(id) }).ToList();
            }
        }

        public List<MyReferendum> GetMyReferendumsDb(string id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<MyReferendum>("SELECT Referendum.Proposition, Referendum.Published, Answer.Appellation, (SELECT Users.UserName FROM Users WHERE Users.Id = AllAnswers.IdUser) AS AnswerAutor FROM Referendum, Answer, AllAnswers, Users WHERE Answer.Referendum = Referendum.id AND AllAnswers.idAnswer = Answer.id AND Users.Id = Referendum.Autor AND Referendum.Autor = @idReferendumAutor", new { idReferendumAutor = Int32.Parse(id) }).ToList();
            }
        }

        public List<MyReferendumNotPublic> GetMyReferendumsNotPublicDb(string id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<MyReferendumNotPublic>("SELECT Referendum.Id, Referendum.Proposition FROM Referendum WHERE Referendum.Published = 0 AND Referendum.Autor = @idReferendumAutor", new { idReferendumAutor = Int32.Parse(id) }).ToList();
            }
        }

        public Referendum GetMyEditableReferendumDb(string id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Referendum>("SELECT Proposition, MaxOwnAnswers, MaxAmountAnswers, DeadLine FROM Referendum WHERE Id = @idReferendum", new { idReferendum = Int32.Parse(id) }).FirstOrDefault();
            }
        }

        public List<AddAnswer> GetMyEditableReferendumAnswers(string id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<AddAnswer>("SELECT Id, Referendum, Appellation FROM Answer WHERE Referendum = @idReferendum", new { idReferendum = Int32.Parse(id) }).ToList();
            }
        }

        public void DeleteAnswerDb(string id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM AllAnswers WHERE IdAnswer = @Id DELETE FROM Answer WHERE Id = @Id";
                db.Execute(sqlQuery, new { Id = Int32.Parse(id) });
            }
        }

        public void ChangePropositionNameDb(ReceiveAllReferendums newName)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Referendum SET Proposition = @Proposition WHERE Id = @Id";
                db.Execute(sqlQuery, newName);
            }
        }

        public void ChangeMaxOwnAnswersDb(Referendum edittedReferendum)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Referendum SET MaxOwnAnswers = @MaxOwnAnswers WHERE Id = @Id";
                db.Execute(sqlQuery, edittedReferendum);
            }
        }

        public void ChangeMaxAmountAnswersDb(Referendum edittedReferendum)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Referendum SET MaxAmountAnswers = @MaxAmountAnswers WHERE Id = @Id";
                db.Execute(sqlQuery, edittedReferendum);
            }
        }

        public void ChangeDeadLineDb(Referendum edittedReferendum)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Referendum SET DeadLine = @DeadLine WHERE Id = @Id";
                db.Execute(sqlQuery, edittedReferendum);
            }
        }

        public void Vote(AllAnswer answer)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO AllAnswers VALUES(@IdAnswer, @IdUser)";
                db.Execute(sqlQuery, answer);
            }
        }

        public void AddReferendum(Referendum referendum)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Referendum VALUES(@Proposition, @Autor, @Active, @Published, @MaxOwnAnswers, @MaxAmountAnswers, @DeadLine, @PublishDate)";
                db.Execute(sqlQuery, referendum);
            }
        }

        public List<ReceiveAllReferendums> ReceiveAllReferendumsDb()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<ReceiveAllReferendums>("SELECT Referendum.Id, Referendum.Proposition FROM Referendum").ToList();
            }
        }

        public void SaveNewAnswer(AddAnswer answer)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Answer VALUES(@Referendum, @Appellation, @AnswerAutor); SELECT CAST(SCOPE_IDENTITY() as int)";
                int? answerId = db.Query<int>(sqlQuery, answer).FirstOrDefault();
                answer.Id = answerId.Value;
                var sqlQuery2 = "INSERT INTO AllAnswers VALUES(@Id, 1)";
                db.Execute(sqlQuery2, answer);
            }
        }
                
        public void Create(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Users VALUES(@UserName, @Password)";
                db.Execute(sqlQuery, user);
            }
        }

        public User Authorize(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT * FROM Users WHERE Users.UserName = @userName AND Users.Password = @Password", new { userName = user.UserName, Password = user.Password }).FirstOrDefault();
            }
        }

        public User Get(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public void Publish(string id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Referendum SET Published = 1 WHERE Id = @id";
                db.Execute(sqlQuery, new { id = Int32.Parse(id) });
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}