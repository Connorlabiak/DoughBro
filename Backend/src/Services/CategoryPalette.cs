using DoughBro.src.Models;

namespace DoughBro.src.Services
{
    public static class CategoryPalette
    {
        public static readonly IReadOnlyList<CategoryColorModel> Colors = new List<CategoryColorModel>
        {
            new() { Id = "tomato", Name = "Tomato", Hex = "#ef4444" },
            new() { Id = "rose", Name = "Rose", Hex = "#f43f5e" },
            new() { Id = "pink", Name = "Pink", Hex = "#ec4899" },
            new() { Id = "violet", Name = "Violet", Hex = "#8b5cf6" },
            new() { Id = "indigo", Name = "Indigo", Hex = "#6366f1" },
            new() { Id = "blue", Name = "Blue", Hex = "#3b82f6" },
            new() { Id = "sky", Name = "Sky", Hex = "#0ea5e9" },
            new() { Id = "cyan", Name = "Cyan", Hex = "#06b6d4" },
            new() { Id = "teal", Name = "Teal", Hex = "#14b8a6" },
            new() { Id = "emerald", Name = "Emerald", Hex = "#10b981" },
            new() { Id = "green", Name = "Green", Hex = "#22c55e" },
            new() { Id = "lime", Name = "Lime", Hex = "#84cc16" },
            new() { Id = "amber", Name = "Amber", Hex = "#f59e0b" },
            new() { Id = "orange", Name = "Orange", Hex = "#f97316" },
            new() { Id = "slate", Name = "Slate", Hex = "#64748b" },
            new() { Id = "stone", Name = "Stone", Hex = "#78716c" },
        };

        public static CategoryColorModel? FindById(string colorId)
        {
            return Colors.FirstOrDefault(color => color.Id.Equals(colorId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
