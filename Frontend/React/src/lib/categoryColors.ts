export const CATEGORY_COLOR_CLASSES = {
    tomato: {
        accent: "bg-red-500",
        card: "border-red-500 bg-red-500/15 shadow-red-500/20",
        selected: "border-red-500 bg-red-500/20 shadow-red-500/30",
        swatch: "bg-red-500",
    },
    rose: {
        accent: "bg-rose-500",
        card: "border-rose-500 bg-rose-500/15 shadow-rose-500/20",
        selected: "border-rose-500 bg-rose-500/20 shadow-rose-500/30",
        swatch: "bg-rose-500",
    },
    pink: {
        accent: "bg-pink-500",
        card: "border-pink-500 bg-pink-500/15 shadow-pink-500/20",
        selected: "border-pink-500 bg-pink-500/20 shadow-pink-500/30",
        swatch: "bg-pink-500",
    },
    violet: {
        accent: "bg-violet-500",
        card: "border-violet-500 bg-violet-500/15 shadow-violet-500/20",
        selected: "border-violet-500 bg-violet-500/20 shadow-violet-500/30",
        swatch: "bg-violet-500",
    },
    indigo: {
        accent: "bg-indigo-500",
        card: "border-indigo-500 bg-indigo-500/15 shadow-indigo-500/20",
        selected: "border-indigo-500 bg-indigo-500/20 shadow-indigo-500/30",
        swatch: "bg-indigo-500",
    },
    blue: {
        accent: "bg-blue-500",
        card: "border-blue-500 bg-blue-500/15 shadow-blue-500/20",
        selected: "border-blue-500 bg-blue-500/20 shadow-blue-500/30",
        swatch: "bg-blue-500",
    },
    sky: {
        accent: "bg-sky-500",
        card: "border-sky-500 bg-sky-500/15 shadow-sky-500/20",
        selected: "border-sky-500 bg-sky-500/20 shadow-sky-500/30",
        swatch: "bg-sky-500",
    },
    cyan: {
        accent: "bg-cyan-500",
        card: "border-cyan-500 bg-cyan-500/15 shadow-cyan-500/20",
        selected: "border-cyan-500 bg-cyan-500/20 shadow-cyan-500/30",
        swatch: "bg-cyan-500",
    },
    teal: {
        accent: "bg-teal-500",
        card: "border-teal-500 bg-teal-500/15 shadow-teal-500/20",
        selected: "border-teal-500 bg-teal-500/20 shadow-teal-500/30",
        swatch: "bg-teal-500",
    },
    yellow: {
        accent: "bg-yellow-500",
        card: "border-yellow-500 bg-yellow-500/15 shadow-yellow-500/20",
        selected: "border-yellow-500 bg-yellow-500/20 shadow-yellow-500/30",
        swatch: "bg-yellow-500",
    },
    green: {
        accent: "bg-green-500",
        card: "border-green-500 bg-green-500/15 shadow-green-500/20",
        selected: "border-green-500 bg-green-500/20 shadow-green-500/30",
        swatch: "bg-green-500",
    },
    lime: {
        accent: "bg-lime-500",
        card: "border-lime-500 bg-lime-500/15 shadow-lime-500/20",
        selected: "border-lime-500 bg-lime-500/20 shadow-lime-500/30",
        swatch: "bg-lime-500",
    },
    amber: {
        accent: "bg-amber-500",
        card: "border-amber-500 bg-amber-500/15 shadow-amber-500/20",
        selected: "border-amber-500 bg-amber-500/20 shadow-amber-500/30",
        swatch: "bg-amber-500",
    },
    orange: {
        accent: "bg-orange-500",
        card: "border-orange-500 bg-orange-500/15 shadow-orange-500/20",
        selected: "border-orange-500 bg-orange-500/20 shadow-orange-500/30",
        swatch: "bg-orange-500",
    },
    slate: {
        accent: "bg-slate-500",
        card: "border-slate-500 bg-slate-500/15 shadow-slate-500/20",
        selected: "border-slate-500 bg-slate-500/20 shadow-slate-500/30",
        swatch: "bg-slate-500",
    },
    stone: {
        accent: "bg-stone-500",
        card: "border-stone-500 bg-stone-500/15 shadow-stone-500/20",
        selected: "border-stone-500 bg-stone-500/20 shadow-stone-500/30",
        swatch: "bg-stone-500",
    },
    charcoal: {
        accent: "bg-zinc-800",
        card: "border-zinc-800 bg-zinc-800/15 shadow-zinc-800/20",
        selected: "border-zinc-800 bg-zinc-800/20 shadow-zinc-800/30",
        swatch: "bg-zinc-800",
    },
} as const;

export type CategoryColorId = keyof typeof CATEGORY_COLOR_CLASSES;

export function getCategoryColorClasses(colorId: string) {
    return CATEGORY_COLOR_CLASSES[colorId as CategoryColorId] ?? CATEGORY_COLOR_CLASSES.slate;
}
