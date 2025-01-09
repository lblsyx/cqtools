function analyseFolder(folderpath, outpath)
{
    //先处理当前目录下的所有文件
    var filelist = FLfile.listFolder(folderpath, "files");
    var dom=fl.createDocument();
    var lib=dom.library;
    for (var i=0; i <filelist.length; ++i)
    {
        var filename=filelist[i];
        var ind=filename.lastIndexOf('.');
        //没有后缀或者后缀不正确则略过
        if (ind <= 0 || ind == (filename.length-1))
            continue;

        //后缀
        var ext=filename.substr(ind+1, filename.length-ind);
        ext = ext.toLowerCase();
        //不是png则略过
        if (ext != "png")
            continue;

        //导出的类名
        var classnameid = filename.substr(0, filename.length - ext.length - 1);
        var realpath = folderpath + "/" + filename;
        //fl.trace(classnameid);
        //fl.trace(realpath);
        dom.importFile(realpath, true);

        //选中此文件，然后设置导出类名。
        lib.selectItem(filename);
        lib.setItemProperty('allowSmoothing', false);
        lib.setItemProperty('compressionType', 'png');
        //lib.setItemProperty('quality', 88)
        lib.setItemProperty("linkageImportForRS",false);
        lib.setItemProperty("linkageExportForAS",true);
        lib.setItemProperty("linkageExportForRS",false);
        lib.setItemProperty("linkageExportInFirstFrame",true);
        lib.setItemProperty('linkageClassName', classnameid);

        //lib.newFolder('_root_');
        //lib.moveToFolder('_root_', filename);
    }
    
    var outName = outpath + "/comp.swf";
    dom.exportSWF(outName);
    //fl.saveDocument(dom, outName.substr(0, outName.length-4)+'.fla');
    dom.close(false);//
    ////处理此目录下的所有文件夹
    //var folderlist = FLfile.listFolder(folderpath, "directories");
    //for (var e=0; e < folderlist.length; ++e)
    //{
    //    analyseFolder(folderpath + "/" + folderlist[e]);
    //}
}

function dotask(dorootdir, outFolder)
{
    for (var i=0; i < fl.documents.length; ++i)
    {
        fl.closeDocument(fl.documents[0], false);
    }
    fl.outputPanel.clear();

    analyseFolder(dorootdir, outFolder);
}

//var temp = fl.browseForFolderURL();
//fl.trace(temp);
var doFolder = 'file:///D|/RXHWPublish/MornAssets';
var outFolder = 'file:///D|/RXHWPublish';
dotask(doFolder, outFolder);