using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TemplateTool.Gens;
using UnityLight.Loggers;

namespace TemplateTool
{
    public class SeqRun
    {
        public enum TASK_ID //枚举需要和checkBoxList索引对应上
        {
            TASK_ID_INVALID =-1,
            TASK_ID_AS3 = 0,
            TASK_ID_CPP,
            TASK_ID_UNITY,
            TASK_ID_PACK,
            TASK_ID_UPLOAD_DB,
            TASK_ID_COMMIT,

            MAX
        }
        private static SeqRun mInstance = new SeqRun();

        public static SeqRun Instance { get { return mInstance; } }
        private int mCurRunIndex = -1;
        private List<TASK_ID> mSeqTaskList = new List<TASK_ID>();
        public void addID(TASK_ID id)
        {
            if (mSeqTaskList.IndexOf(id) == -1)
            {
                mSeqTaskList.Add(id);
            }
            else
            {
                XLogger.WarnFormat("addID 已存在的ID", id);
            }
        }
        public TASK_ID getCurID()
        {
            if(mCurRunIndex>=0&& mCurRunIndex < mSeqTaskList.Count)
            {
                return mSeqTaskList[mCurRunIndex];

            }
            return TASK_ID.TASK_ID_INVALID;
        }
        public void clearID()
        {
            mSeqTaskList.Clear();
        }
        public void removeID(TASK_ID id)
        {
            mSeqTaskList.Remove(id);
        }
        public void runID(TASK_ID id)
        {
            var name = getTaskName(id);
            XLogger.InfoFormat("开始执行：{0}", name);
            mForm.RunTask(id);
        }
        public static int getGenCodeTypeByTaskID(TASK_ID id)
        {
            switch (id)
            {
                case TASK_ID.TASK_ID_AS3:
                    return (int)GeneratorType.AS3Client;

                case TASK_ID.TASK_ID_CPP:
                    return (int)GeneratorType.CPPServer;

                case TASK_ID.TASK_ID_UNITY:
                    return (int)GeneratorType.Unity3DClient;

            }
            return -1;
        }
        /// <summary>
        /// 根据枚举值获得generatorTypeComboBox下拉框的索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int getComboBoxItemIdxByTaskID(TASK_ID id)
        {
            switch (id)
            {
                case TASK_ID.TASK_ID_AS3:
                    return 0;

                case TASK_ID.TASK_ID_CPP:
                    return 1;

                case TASK_ID.TASK_ID_UNITY:
                    return 2;

            }
            return -1;
        }
        public static string getTaskName(TASK_ID id)
        {
            var str = String.Empty;
            switch (id)
            {
                case TASK_ID.TASK_ID_AS3:
                    str = "生成AS3代码";
                    break;
                case TASK_ID.TASK_ID_CPP:
                    str = "生成CPP代码";
                    break;
                case TASK_ID.TASK_ID_UNITY:
                    str = "生成Unity代码";
                    break;
                case TASK_ID.TASK_ID_PACK:
                    str = "打包数据";
                    break;
                case TASK_ID.TASK_ID_UPLOAD_DB:
                    str = "上传数据库";
                    break;
                default:
                    break;

            }
            return str;
        }

        public bool isRunning()
        {
            return mCurRunIndex > -1 && mCurRunIndex < mSeqTaskList.Count;
        }

        private MainForm mForm = null;
        public void startRun(MainForm form)
        {
            mForm = form;
            if (mSeqTaskList.Count == 0)
            {
                return;
            }
            if (isRunning())
            {
                return;
            }
            mCurRunIndex = 0;
            var curID = mSeqTaskList[mCurRunIndex];
            runID(curID);
        }
        public void stop()
        {
            mSeqTaskList.Clear();
            mCurRunIndex = -1;
            mForm.StopTask();
        }

        private void runNext()
        {
            if (!isRunning())
            {
                return;
            }
            var nextIndex = mCurRunIndex + 1;
            if (nextIndex >= mSeqTaskList.Count)
            {
                onAllTaskFinished();
                return;
            }
            mCurRunIndex = nextIndex;
            var curID = mSeqTaskList[mCurRunIndex];
            runID(curID);
        }
        public void onCurTaskFinished(bool succeed = true,bool failedStop = true)
        {
            var curID = mSeqTaskList[mCurRunIndex];
            var name = getTaskName(curID);
            mForm.setTaskFinish(curID, succeed);

            if (succeed)
            {
                XLogger.InfoFormat("{0} 执行结束：成功！", name);
                runNext();
            }
            else
            {
                XLogger.InfoFormat("{0} 执行结束：失败！", name);
                if (failedStop) //失败后停止
                {
                    stop();
                }
            }

        }
        public void onAllTaskFinished()
        {
            XLogger.Info("一键执行文件生成完毕！");
            //这里暂时写死，懒得扩展了
            mForm.OnAllTasksFinished();
            stop();
            MessageBox.Show("一键执行完毕！");

        }


    }
}
