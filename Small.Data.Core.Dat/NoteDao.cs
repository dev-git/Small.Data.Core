using Small.Data.Core.Blt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Small.Data.Core.Dat
{
    public class NoteDao
    {
        public int Insert(Note note)
        {
            /*"create table Movie (movie_pk integer primary key autoincrement, movie_date datetime, " +
					 " movie_title text, movie_desc text, movie_genre test, when_created datetime default (strftime('%Y-%m-%d %H:%M:%f', 'now')), " +
					 " is_deleted bool default (0), user_modified int default (0));";*/

            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into SimpleNote (note_name, note_detail, note_category) values ('");
            sql.Append((String.IsNullOrEmpty(note.Name) ? DateTime.Now.ToString("yyyyMMddHHmmss") : note.Name.Replace("'", "\"")) + "', '");
            sql.Append((String.IsNullOrEmpty(note.Detail) ? String.Empty : note.Detail.Replace("'", "\"")) + "', '");
            sql.Append((String.IsNullOrEmpty(note.Category) ? String.Empty : note.Category.Replace("'", "\"")) + "' );");

            //sql.Append(movie.Title.Replace("'", "\"") + "');");

            //int newId = Db.Insert(sql.ToString(), true);

            return 1;// newId;
        }

        public IList<Note> GetNotes()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select note_pk, note_name, note_detail, note_category, datetime(when_created, 'localtime') as when_created from SimpleNote ");
            sql.Append(" where is_deleted = 0 order by when_created desc;");

            DataTable dt = new DataTable();// Db.GetDataTable(sql.ToString());

            return MakeNoteList(dt);
        }

        private IList<Note> MakeNoteList(DataTable dt)
        {
            IList<Note> list = new List<Note>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(MakeNote(dr));
            }

            return list;
        }

        private Note MakeNote(DataRow dr)
        {

            Note myNote = new Note();
            myNote.Id = Int32.Parse(dr["note_pk"].ToString());
            myNote.Name = dr["note_name"].ToString();
            myNote.Detail = dr["note_detail"].ToString();
            myNote.Category = dr["note_category"].ToString();
            myNote.CreateDate = DateTime.Parse(dr["when_created"].ToString());

            return myNote;
        }


        public void Delete(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(String.Format("update SimpleNote set is_deleted = 1 where note_pk = {0};", id.ToString()));
            //Db.GetScalar(sql.ToString());

        }
    }
}
