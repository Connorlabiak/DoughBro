import { format, parseISO } from "date-fns"
import { RiCalendarLine } from "react-icons/ri"

import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover"

interface DatePickerProps {
    value?: string
    onChange: (date: string) => void
    placeholder?: string
    className?: string
    displayFormat?: string
}

export function DatePicker({
    value,
    onChange,
    placeholder = "Pick a date",
    className,
    displayFormat = "MM/dd/yyyy",
}: DatePickerProps) {
    const selectedDate = value ? parseISO(value) : undefined

    return (
        <Popover>
            <PopoverTrigger
                render={
                    <Button
                        variant="ghost"
                        className={cn(
                            "h-auto w-fit p-0 normal-case font-normal tracking-normal hover:bg-transparent text-xs text-zinc-500 focus-visible:ring-0",
                            !value && "text-zinc-400",
                            className
                        )}
                    >
                        <span className="font-sans font-medium">
                            {selectedDate ? format(selectedDate, displayFormat) : placeholder}
                        </span>
                        <RiCalendarLine className="ml-1 size-3.5 shrink-0 text-zinc-500" />
                    </Button>
                }
            />

            <PopoverContent
                className="z-50 w-auto rounded-md border border-zinc-200 bg-white p-2 shadow-lg"
                align="start"
                sideOffset={6}
            >
                <Calendar
                    mode="single"
                    selected={selectedDate}
                    onSelect={(date) => {
                        if (date) {
                            onChange(format(date, "yyyy-MM-dd"))
                        }
                    }}
                    className="[&_.rdp-day_selected]:bg-[#BF00FF] [&_.rdp-day_selected]:text-white"
                />
            </PopoverContent>
        </Popover>
    )
}