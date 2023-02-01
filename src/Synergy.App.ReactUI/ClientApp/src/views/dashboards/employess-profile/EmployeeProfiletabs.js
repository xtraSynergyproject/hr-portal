import * as React from 'react';
import PropTypes from 'prop-types';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Personalinfo from './PersonalInfo/personalinfo';
import AssignmentForm from './AssignmentForm';
import EmplyessPaySlip from './PersonalInfo/EmplyessPaySlip';
import Emplyesspayroll from "./PersonalInfo/Emplyesspayroll"
import Etable from "./PersonalInfo/Etable"
import ContactEmplyess from "./PersonalInfo/ContactEmplyess"
import AttendenceE from './PersonalInfo/AttendenceE'
import Deparment from './PersonalInfo/Deparment'

function TabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

TabPanel.propTypes = {
  children: PropTypes.node,
  index: PropTypes.number.isRequired,
  value: PropTypes.number.isRequired,
};

function a11yProps(index) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

export default function EmployeeProfiletabs() {
  const [value, setValue] = React.useState(0);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  return (
    <Box sx={{ width: '100%' }}>
      <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Tabs value={value} onChange={handleChange} aria-label="basic tabs example">
          <Tab label="Personal Info" {...a11yProps(0)} />
          <Tab label="Assignment" {...a11yProps(1)} />
          <Tab label="Contact" {...a11yProps(2)} />
          <Tab label="Leave" {...a11yProps(3)} />
          <Tab label="Attendance" {...a11yProps(4)} />
          <Tab label="Documents" {...a11yProps(5)} />
          <Tab label="Deparment" {...a11yProps(6)} />
          <Tab label="Payroll" {...a11yProps(7)} />
          <Tab label="Pay Slip" {...a11yProps(8)} />
        </Tabs>
      </Box>
      <TabPanel value={value} index={0}>
        <Personalinfo />
      </TabPanel>
      <TabPanel value={value} index={1}>
        <AssignmentForm />
      </TabPanel>
      <TabPanel value={value} index={2}>
        <ContactEmplyess/>
      </TabPanel>
      <TabPanel value={value} index={3}>
      <Etable/>
      </TabPanel>
      <TabPanel value={value} index={4}>
        <AttendenceE/>
      </TabPanel>
      <TabPanel value={value} index={5}>
      <Etable/>
      </TabPanel>
      <TabPanel value={value} index={6}>
      <Etable/>
      </TabPanel>
      <TabPanel value={value} index={7}>
        <EmplyessPaySlip/>
      </TabPanel>
      <TabPanel value={value} index={8}>
        <Emplyesspayroll/>
      </TabPanel>
      <TabPanel value={value} index={8}>
       
      </TabPanel>
    </Box>
  );
}
