using UnityEngine;
using UnityEngine.UI;

public class SpellCooldown : MonoBehaviour
{
    [Header("UI Components")]
    public Image dashIcon;
    public Image levitateIcon;
    public Image fillImage; // Fill image du Dash
    public Image levitateFillImage; // Fill image de la Lévitation
    public Text dashTimerText; // Timer pour le Dash
    public Text levitateTimerText; // Timer pour la Lévitation

    private CharacterManager2D manager;
    private float dashCooldownTimer;
    private float levitationCooldownTimer;

    private void Start()
    {
        // Récupère le manager du personnage
        manager = FindFirstObjectByType<CharacterManager2D>();

        // Initialisation des timers de cooldown
        dashCooldownTimer = manager.dashingCooldown;
        levitationCooldownTimer = manager.levitationCooldown;
    }

    private void Update()
    {
        // Gérer le cooldown du Dash
        HandleDashCooldown();

        // Gérer le cooldown de la Lévitation
        HandleLevitationCooldown();
    }

    private void HandleDashCooldown()
    {
        if (manager.canDash)
        {
            // Si le dash est disponible, on reset le cooldown et on masque l'UI
            fillImage.fillAmount = 0f;
            dashTimerText.text = ""; // Masque le timer du dash
            return;
        }

        // Sinon, on calcule le cooldown restant pour le dash
        dashCooldownTimer -= Time.deltaTime;

        // Remplissage radial basé sur le temps restant
        float fillAmount = Mathf.Clamp01(dashCooldownTimer / manager.dashingCooldown);
        fillImage.fillAmount = fillAmount;

        // Affichage du timer du dash sous forme de texte
        dashTimerText.text = Mathf.Ceil(dashCooldownTimer).ToString();

        // Quand le cooldown est terminé, on réactive le dash
        if (dashCooldownTimer <= 0f)
        {
            manager.canDash = true;
            dashCooldownTimer = manager.dashingCooldown;
        }
    }

    private void HandleLevitationCooldown()
    {
        if (manager.canLevitate)
        {
            // Si la lévitation est disponible, on reset le cooldown et on masque l'UI
            levitateFillImage.fillAmount = 0f;
            levitateTimerText.text = ""; // Masque le timer de la lévitation
            return;
        }

        // Sinon, on calcule le cooldown restant pour la lévitation
        levitationCooldownTimer -= Time.deltaTime;

        // Remplissage radial basé sur le temps restant
        float fillAmount = Mathf.Clamp01(levitationCooldownTimer / manager.levitationCooldown);
        levitateFillImage.fillAmount = fillAmount;

        // Affichage du timer de la lévitation sous forme de texte
        levitateTimerText.text = Mathf.Ceil(levitationCooldownTimer).ToString();

        // Quand le cooldown est terminé, on réactive la lévitation
        if (levitationCooldownTimer <= 0f)
        {
            manager.canLevitate = true;
            levitationCooldownTimer = manager.levitationCooldown;
        }
    }
}
