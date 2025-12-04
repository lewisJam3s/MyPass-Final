namespace MyPass.Models.ViewModels.Vault
{
    public class SecureNoteViewModel
    {
        public int Id { get; set; }
        public string NoteTitle { get; set; } = string.Empty;
        public string NoteContent { get; set; } = string.Empty;
    }

}
