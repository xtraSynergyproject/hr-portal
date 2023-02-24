// ** MUI Imports
import { useState } from 'react'
import Tab from '@mui/material/Tab'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import Typography from '@mui/material/Typography'
import Personalinfo from './PersonalInfo/personalinfo';
import AssignmentForm from './AssignmentForm';
import PayrollPaySlip from 'src/views/dashboards/payroll/components/PayrollPaySlip';
import PayrollSalaryInfo from "src/views/dashboards/payroll/components/PayrollSalaryInfo"
import Leave_Pages from "./PersonalInfo/Leave_Pages"
import ContactEmplyess from "./PersonalInfo/ContactEmplyess"
import AttendenceE from './PersonalInfo/AttendenceE'
import Document_Pages from './PersonalInfo/Document_Pages'
import Dependents_Pages from './PersonalInfo/Dependents_Pages'
const TabsForcedScroll = () => {
  // ** State
  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  return (
    <TabContext value={value}>
      <TabList scrollButtons variant='scrollable' onChange={handleChange} aria-label='forced scroll tabs example'>
        <Tab value='1' label='Personal Info'  />
        <Tab value='2' label='Assignment'  />
        <Tab value='3' label='Contract' />
        <Tab value='4' label='Leave'  />
        <Tab value='5' label='Attendance'  />
        <Tab value='6' label='Documents' />
        <Tab value='7' label='Dependents' />
        <Tab value='8' label='Payroll Salary Info'  />
        <Tab value='9' label='Pay Slip'  />
      </TabList>
      <TabPanel value='1'>
        <Typography>
        <Personalinfo />
        </Typography>
      </TabPanel>
      <TabPanel value='2'>
        <Typography> 
          <AssignmentForm />
        </Typography>
      </TabPanel>
      <TabPanel value='3'>
        <Typography>
          <ContactEmplyess />
        </Typography>
      </TabPanel>
      <TabPanel value='4'>
        <Typography>
          <Leave_Pages />
        </Typography>
      </TabPanel>
      <TabPanel value='5'>
        <Typography>
          <AttendenceE />
        </Typography>
      </TabPanel>
      <TabPanel value='6'>
        <Typography>
        <Document_Pages />
        </Typography>
      </TabPanel>
      <TabPanel value='7'>
        <Typography>
        <Dependents_Pages />
        </Typography>
      </TabPanel>
      <TabPanel value='8'>
        <Typography>
        <PayrollSalaryInfo />
        </Typography>
      </TabPanel>
      <TabPanel value='9'>
        <Typography>
        <PayrollPaySlip />
        </Typography>
      </TabPanel>
    </TabContext>
  )
}

export default TabsForcedScroll
