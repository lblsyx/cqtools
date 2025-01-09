using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProtocolClient
{
    public class RowDragDrop<T> where T : class, ICloneable
    {
        private int mDragIndex;
        private DataGridView mDataGridView;
        public RowDragDrop(DataGridView oDataGridView)
        {
            mDragIndex = -1;
            mDataGridView = oDataGridView;

            RegisterDragEvent();
        }

        public void RegisterDragEvent()
        {
            mDataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(mDataGridView_CellMouseDown);
            mDataGridView.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(mDataGridView_CellMouseMove);
            mDataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(mDataGridView_DragEnter);
            mDataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(mDataGridView_DragDrop);
            mDataGridView.DragLeave += new System.EventHandler(mDataGridView_DragLeave);
        }
        
        public void UnregisterDragEvent()
        {
            mDataGridView.CellMouseDown -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(mDataGridView_CellMouseDown);
            mDataGridView.CellMouseMove -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(mDataGridView_CellMouseMove);
            mDataGridView.DragEnter -= new System.Windows.Forms.DragEventHandler(mDataGridView_DragEnter);
            mDataGridView.DragDrop -= new System.Windows.Forms.DragEventHandler(mDataGridView_DragDrop);
            mDataGridView.DragLeave -= new System.EventHandler(mDataGridView_DragLeave);
        }

        private void mDataGridView_DragLeave(object sender, EventArgs e)
        {
            mDataGridView.AllowDrop = false;
        }

        private void mDataGridView_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(mDataGridView, e.X, e.Y);

            if (idx < 0 || idx == mDragIndex) return;

            BindingList<T> list = mDataGridView.DataSource as BindingList<T>;

            T t = list[mDragIndex];
            T newT = t.Clone() as T;
            list.Insert(idx, newT);
            list.Remove(t);

            if (mDragIndex < idx) mDragIndex = idx - 1;
            else mDragIndex = idx;

            mDataGridView.CurrentCell = mDataGridView.Rows[mDragIndex].Cells[0];
            for (int i = 0; i < mDataGridView.Rows[mDragIndex].Cells.Count; i++)
            {
                mDataGridView.Rows[mDragIndex].Cells[i].Selected = true;
            }
        }

        private void mDataGridView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void mDataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnIndex == -1 && e.RowIndex > -1 && mDataGridView.Rows[e.RowIndex].DataBoundItem != null)
            {
                mDataGridView.DoDragDrop(mDataGridView.Rows[e.RowIndex].DataBoundItem, DragDropEffects.Move);
            }
        }

        private void mDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            mDragIndex = e.RowIndex;
            mDataGridView.AllowDrop = true;
        }

        private static int GetRowFromPoint(DataGridView dataGridView, int x, int y)
        {
            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                Rectangle rec = dataGridView.GetRowDisplayRectangle(i, false);

                if (dataGridView.RectangleToScreen(rec).Contains(x, y))
                    return i;
            }

            return -1;
        }
    }
}
