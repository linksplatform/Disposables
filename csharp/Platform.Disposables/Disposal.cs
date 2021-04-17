namespace Platform.Disposables
{
    /// <summary>
    /// <para>Encapsulates a method that is used to dispose unmanaged resources.</para>
    /// <para>Инкапсулирует метод, который используется для высвобождения неуправляемых ресурсов.</para>
    /// </summary>
    /// <param name="manual">
    /// <para>A value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from a developer.</para>
    /// <para>Значение определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
    /// </param>
    /// <param name="wasDisposed">
    /// <para>A value that determines whether the object was released before calling this method.</para>
    /// <para>Значение определяющие был ли высвобожден объект до вызова этого метода.</para>
    /// </param>
    public delegate void Disposal(bool manual, bool wasDisposed);
}
