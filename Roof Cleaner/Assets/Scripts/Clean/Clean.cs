using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Clean : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [Header("Textures")]
    [SerializeField] private Texture2D _dirtMaskBase; // precisa ser toda verde (G=1)
    [SerializeField] private Texture2D _brush;        // precisa ter canal G com variação (0~1)

    [Header("Material Template")]
    [SerializeField] private Material _materialTemplate; // Material com shader que usa "_DirtMask"

    private Material _materialInstance;
    private Texture2D _dirtMaskInstance;

    private void Start()
    {
        // Clona o material original (evita compartilhamento entre objetos)
        _materialInstance = new Material(_materialTemplate);

        // Aplica o material no renderer deste objeto
        GetComponent<Renderer>().material = _materialInstance;

        // Cria uma nova textura copiando a base (também única para este objeto)
        CreateTexture();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Lança um raio da câmera para onde o mouse está
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                // Garante que foi este objeto que recebeu o clique
                if (hit.collider.gameObject != gameObject) return;

                Vector2 uv = hit.textureCoord;

                int pixelX = (int)(uv.x * _dirtMaskInstance.width);
                int pixelY = (int)(uv.y * _dirtMaskInstance.height);

                int startX = pixelX - _brush.width / 2;
                int startY = pixelY - _brush.height / 2;

                for (int x = 0; x < _brush.width; x++)
                {
                    for (int y = 0; y < _brush.height; y++)
                    {
                        int targetX = startX + x;
                        int targetY = startY + y;

                        if (targetX >= 0 && targetX < _dirtMaskInstance.width &&
                            targetY >= 0 && targetY < _dirtMaskInstance.height)
                        {
                            Color brushPixel = _brush.GetPixel(x, y);
                            Color maskPixel = _dirtMaskInstance.GetPixel(targetX, targetY);

                            float newG = maskPixel.g - brushPixel.g;
                            newG = Mathf.Clamp01(newG);

                            _dirtMaskInstance.SetPixel(targetX, targetY, new Color(0, newG, 0));
                        }
                    }
                }

                _dirtMaskInstance.Apply();
            }
        }
    }

    private void CreateTexture()
    {
        _dirtMaskInstance = new Texture2D(_dirtMaskBase.width, _dirtMaskBase.height, TextureFormat.RGBA32, false);
        _dirtMaskInstance.SetPixels(_dirtMaskBase.GetPixels());
        _dirtMaskInstance.Apply();

        _materialInstance.SetTexture("_DirtMask", _dirtMaskInstance);
    }
}
