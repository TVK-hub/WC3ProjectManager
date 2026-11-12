namespace WC3ProjectManager.Project
{
    public class WC3ProjectDirectories
    {
        //Проект
        WC3Project Project;

        //Директории
        public string Build     => Path.Combine(Project.Path, "Build");
        public string Maps      => Path.Combine(Project.Path, "Maps");
        public string Objects   => Path.Combine(Project.Path, "Objects");
        public string Triggers  => Path.Combine(Project.Path, "Triggers");
        public string Import    => Path.Combine(Project.Path, "Import");

        //Конструктор
        public WC3ProjectDirectories(WC3Project prj)
        {
            Project = prj;
        }
    }
}