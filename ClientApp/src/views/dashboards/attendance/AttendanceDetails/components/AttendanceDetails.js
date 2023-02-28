import React from 'react'
import AttendanceCalender from '../AttendanceCalender'
import UserProfile from 'src/views/dashboards/payroll/components/UserProfile'
function AttendanceDetails() {
  return (
    <div><UserProfile/>
      <AttendanceCalender/></div>
  )
}

export default AttendanceDetails