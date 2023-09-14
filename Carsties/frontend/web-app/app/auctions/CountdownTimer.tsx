'use client'

import React from 'react'
import Countdown, { zeroPad } from 'react-countdown';

type Props = {
    auctionEnd: string;
}

const customRenderer = ({ days, hours, minutes, seconds, complited }: 
    { days: number, hours: number, minutes: number, seconds: number, complited: number }) => {
    return (
        <div className={`border-2 
                         border-white 
                         text-white py-1 px-2 
                         rounded-lg 
                         flex justify-center
                        ${complited ? 
                            'bg-red-600' : (days == 0 && hours < 10)
                            ? 'bg-amber-600' : 'bg-green-600'}
                        `}>
            {complited ? (
                <span>Auction finished</span>
            ) : (
                <span suppressHydrationWarning={true}>
                    {days}D {zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
                </span>
            )}
        </div>
    )
}

export default function CountdownTimer({auctionEnd}: Props) {
  return (
    <div>
        <Countdown date={auctionEnd} renderer={customRenderer} />
    </div>
  )
}
