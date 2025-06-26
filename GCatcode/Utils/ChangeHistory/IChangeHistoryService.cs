using Resser.Data.DTOs;
using System.Collections.Generic;

public interface IChangeHistoryService
{
    /// <summary>
    /// Save Change History.
    /// Usar despues de Context.SaveChanges();,  en caso de existir.
    /// </summary>
    /// <param name="observations">Other mensajes text.</param>
    List<ChangeHistoryDTO> SaveHistory(string observations);

    List<ChangeHistoryDTO> ReverseHistory(int changeHistoryId);
    object GetReverse(int changeHistoryId);
}